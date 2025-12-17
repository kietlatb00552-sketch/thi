using ASM_C_6_API.Data;
using ASM_C_6_API.Model;
using ASM_C_6_API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    // GIẢI PHÁP 1: Bỏ qua cycles (mặc định cho .NET 8+)
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

    // Hoặc GIẢI PHÁP 2: Preserve references (thêm $id và $ref)
    // options.SerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;

    // Tùy chọn thêm
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.WriteIndented = true; // Pretty print
});
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "ASM C#6 API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddScoped<IJwtServices, JwtServices>();
builder.Services.AddScoped<IFoodItemServices, FoodItemServices>();
builder.Services.AddScoped<ICategoryServices, CategoryServices>();
builder.Services.AddScoped<IComboServices, ComboServices>();
builder.Services.AddScoped<IComboDetailServices, ComboDetailServices>();
builder.Services.AddScoped<ICartServices, CartServices>();
builder.Services.AddScoped<ICartItemServices, CartItemServices>();
builder.Services.AddScoped<IOrderServices, OrderServices>();
builder.Services.AddScoped<IOrderDetailServices, OrderDetailServices>();
builder.Services.AddScoped<IUserServices, UserServices>();

var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["Key"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero // Không có thời gian buffer
    };
});

builder.Services.AddAuthorization(options =>
{
    // Policy cho Admin
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));

    // Policy cho User hoặc Admin
    options.AddPolicy("UserOrAdmin", policy =>
        policy.RequireRole("User", "Admin"));

    // Policy custom với claims
    options.AddPolicy("CanManageProducts", policy =>
        policy.RequireClaim("Permission", "ManageProducts"));

    // Policy với nhiều điều kiện
    options.AddPolicy("SeniorAdmin", policy =>
    {
        policy.RequireRole("Admin");
        policy.RequireClaim("Seniority", "Senior");
    });
});

// Đăng ký CORS policy và chỉ cho phép origin dev của bạn
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalDev", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // <-- origin front-end
              .AllowAnyHeader()
              .AllowAnyMethod();
        // .AllowCredentials(); // Bật nếu bạn gửi cookies/credentials. Nếu bật, không dùng AllowAnyOrigin.
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ThỨ tự quan trọng: gọi UseCors trước MapControllers/MapEndpoints
app.UseCors("AllowLocalDev");

app.MapGet("/", () => Results.Redirect("/swagger"));

//Role 
app.MapGet("/api/roles", async (ApplicationDbContext context) =>
{
    var roles = await context.Roles.ToListAsync();
    return Results.Ok(roles);
}).AllowAnonymous();

//Category API
app.MapGet("/api/categories", async (ICategoryServices categoryServices) =>
{
    var categories = categoryServices.GetAllCategories();
    return Results.Ok(categories);
}).AllowAnonymous();

app.MapPost("/api/categories", async (ICategoryServices categoryServices, Category category) =>
{
    categoryServices.AddCategory(category);
    return Results.Created($"/api/categories/{category.CategoryId}", category);
});

app.MapPut("/api/categories/{id}", async (ICategoryServices categoryServices, int id, Category category) =>
{
    if (id != category.CategoryId)
    {
        return Results.BadRequest();
    }
    categoryServices.UpdateCategory(category);
    return Results.NoContent();
});

app.MapDelete("/api/categories/{id}", async (ICategoryServices categoryServices, int id) =>
{
    categoryServices.DeleteCategory(id);
    return Results.NoContent();
});

//Food APi
app.MapGet("/api/fooditems", async (IFoodItemServices foodItemServices) =>
{
    var foodItems = foodItemServices.GetAllFoodItems();
    return Results.Ok(foodItems);
}).AllowAnonymous();

app.MapPost("/api/fooditems", async (IFoodItemServices foodItemServices, FoodItem foodItem) =>
{
    foodItemServices.AddFoodItem(foodItem);
    return Results.Created($"/api/fooditems/{foodItem.FoodItemId}", foodItem);
});
//.RequireAuthorization("AdminOnly");

app.MapPut("/api/fooditems/{id}", async (IFoodItemServices foodItemServices, int id, FoodItem foodItem) =>
{
    if (id != foodItem.FoodItemId)
    {
        return Results.BadRequest();
    }
    foodItemServices.UpdateFoodItem(foodItem);
    return Results.NoContent();
});

app.MapDelete("/api/fooditems/{id}", async (IFoodItemServices foodItemServices, int id) =>
{
    foodItemServices.DeleteFoodItem(id);
    return Results.NoContent();
});

//Combo API, Cart API, Order API can be added similarly

app.MapGet("/api/combos", async (IComboServices comboServices) =>
{
    var combos = comboServices.GetAllCombos();
    return Results.Ok(combos);
});

app.MapPost("/api/combos", async (IComboServices comboServices, Combo combo) =>
{
    comboServices.AddCombo(combo);
    return Results.Created($"/api/combos/{combo.ComboId}", combo);
});

app.MapPut("/api/combos/{id}", async (IComboServices comboServices, int id, Combo combo) =>
{
    if (id != combo.ComboId)
    {
        return Results.BadRequest();
    }
    comboServices.UpdateCombo(combo);
    return Results.NoContent();
});

app.MapDelete("/api/combos/{id}", async (IComboServices comboServices, int id) =>
{
    comboServices.DeleteCombo(id);
    return Results.NoContent();
});

//Combo Detail API, Cart API, Cart Item API, Order API, Order Detail API can be added similarly
app.MapGet("/api/combodetails", async (IComboDetailServices comboDetailServices) =>
{
    var comboDetails = comboDetailServices.GetAllComboDetails();
    return Results.Ok(comboDetails);
});

app.MapPost("/api/combodetails", async (IComboDetailServices comboDetailServices, ComboItem comboDetail, int comboId) =>
{
    comboDetailServices.AddComboDetail(comboDetail, comboId);
    return Results.Created($"/api/combodetails/{comboDetail.ComboItemId}", comboDetail);
});

app.MapPut("/api/combodetails/{id}", async (IComboDetailServices comboDetailServices, int id, ComboItem comboDetail) =>
{
    if (id != comboDetail.ComboItemId)
    {
        return Results.BadRequest();
    }
    // Assuming UpdateComboDetail method exists
    // comboDetailServices.UpdateComboDetail(comboDetail);
    return Results.NoContent();
});

app.MapDelete("/api/combodetails/{id}", async (IComboDetailServices comboDetailServices, int id) =>
{
    var comboDetail = comboDetailServices.GetAllComboDetails().FirstOrDefault(cd => cd.ComboItemId == id);
    if (comboDetail == null)
    {
        return Results.NotFound();
    }
    comboDetailServices.RemoveComboDetail(comboDetail);
    return Results.NoContent();
});

//Order and Cart APIs would follow a similar pattern
app.MapGet("/api/carts", async (ICartServices cartServices) =>
{
    var carts = cartServices.GetAllCarts();
    return Results.Ok(carts);
});

app.MapGet("/api/user/{id}/carts", async (ICartServices cartServices, int id) =>
{
    var listUserCart = cartServices.GetCartsByUserId(id);
    return Results.Ok(listUserCart);
}).RequireAuthorization();

app.MapPost("/api/carts", async (ICartServices cartServices, Cart cart) =>
{
    cartServices.AddCart(cart);
    return Results.Created($"/api/carts/{cart.CartId}", cart);
});

app.MapPut("/api/carts/{id}", async (ICartServices cartServices, int id, Cart cart) =>
{
    if (id != cart.CartId)
    {
        return Results.BadRequest();
    }
    cartServices.UpdateCart(cart);
    return Results.NoContent();
});

app.MapDelete("/api/carts/{id}", async (ICartServices cartServices, int id) =>
{
    cartServices.DeleteCart(id);
    return Results.NoContent();
});

//Cart Item APIs
app.MapGet("/api/cartitems", async (ICartItemServices cartItemServices) =>
{
    var cartItems = cartItemServices.GetAllCartItems();
    return Results.Ok(cartItems);
});

app.MapPost("/api/cartitems", async (ICartItemServices cartItemServices, CartItem cartItem, int cartId) =>
{
    cartItemServices.AddCartItem(cartItem, cartId);
    return Results.Created($"/api/cartitems/{cartItem.CartItemId}", cartItem);
});

app.MapPut("/api/cartitems/{id}", async (ICartItemServices cartItemServices, int id, CartItem cartItem) =>
{
    if (id != cartItem.CartItemId)
    {
        return Results.BadRequest();
    }
    cartItemServices.UpdateCartItem(cartItem);
    return Results.NoContent();
});

app.MapDelete("/api/cartitems/{id}", async (ICartItemServices cartItemServices, int id) =>
{
    var cartItem = cartItemServices.GetAllCartItems().FirstOrDefault(ci => ci.CartItemId == id);
    if (cartItem == null)
    {
        return Results.NotFound();
    }
    cartItemServices.RemoveCartItem(cartItem);
    return Results.NoContent();
});

//Order APIs
app.MapGet("/api/orders", async (IOrderServices orderServices) =>
{
    var orders = orderServices.GetAllOrders();
    return Results.Ok(orders);
});

app.MapPost("/api/orders", async (IOrderServices orderServices, Order order) =>
{
    orderServices.AddOrder(order);
    return Results.Created($"/api/orders/{order.OrderId}", order);
});

app.MapPut("/api/orders/{id}", async (IOrderServices orderServices, int id, Order order) =>
{
    if (id != order.OrderId)
    {
        return Results.BadRequest();
    }
    orderServices.UpdateOrder(order);
    return Results.NoContent();
});

app.MapDelete("/api/orders/{id}", async (IOrderServices orderServices, int id) =>
{
    orderServices.DeleteOrder(id);
    return Results.NoContent();
});

//Order Detail APIs can be added similarly
app.MapGet("/api/orderdetails", async (IOrderDetailServices orderDetailServices) =>
{
    var orderDetails = orderDetailServices.GetAllOrderDetails();
    return Results.Ok(orderDetails);
});

app.MapPost("/api/orderdetails", async (IOrderDetailServices orderDetailServices, OrderDetail orderDetail, int orderId) =>
{
    orderDetailServices.AddOrderDetail(orderDetail, orderId);
    return Results.Created($"/api/orderdetails/{orderDetail.OrderDetailId}", orderDetail);
});

app.MapDelete("/api/orderdetails/{id}", async (IOrderDetailServices orderDetailServices, int id) =>
{
    var orderDetail = orderDetailServices.GetAllOrderDetails().FirstOrDefault(od => od.OrderDetailId == id);
    if (orderDetail == null)
    {
        return Results.NotFound();
    }
    orderDetailServices.RemoveOrderDetail(orderDetail);
    return Results.NoContent();
});

//User Management APIs can be added similarly
app.MapGet("/api/users", async (IUserServices userServices) =>
{
    var users = userServices.GetAllUsers();
    return Results.Ok(users);
});

app.MapPost("/api/users", async (IUserServices userServices, User user) =>
{
    userServices.AddUser(user);
    return Results.Created($"/api/users/{user.UserId}", user);
});

app.MapPut("/api/users/{id}", async (IUserServices userServices, int id, User user) =>
{
    if (id != user.UserId)
    {
        return Results.BadRequest();
    }
    userServices.UpdateUser(user);
    return Results.NoContent();
});

app.MapDelete("/api/users/{id}", async (IUserServices userServices, int id) =>
{
    userServices.DeleteUser(id);
    return Results.NoContent();
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
