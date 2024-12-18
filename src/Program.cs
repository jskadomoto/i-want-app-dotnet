var builder = WebApplication.CreateBuilder(args);

#region Builders

builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["ConnectionString:IWantDb"]);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddScoped<QueryAllUsersWithClaimName>();
builder.Services.AddScoped<QueryAllProductsSold>();
builder.Services.AddScoped<UserCreator>();
builder.Services.AddJwtAuthentication(builder.Configuration); /* Authentication */
builder.Services.AddCustomAuthorization(); /* Requires authenticated user by default */
builder.Host.AddSerilogLogging(builder.Configuration);

#endregion Builders

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

#region Routes
/* Products */
app.MapMethods(ProductPost.Template, ProductPost.Methods, ProductPost.Handler);
app.MapMethods(ProductGetAll.Template, ProductGetAll.Methods, ProductGetAll.Handler);
app.MapMethods(ProductSoldGet.Template, ProductSoldGet.Methods, ProductSoldGet.Handler);
app.MapMethods(ProductGetShowCase.Template, ProductGetShowCase.Methods, ProductGetShowCase.Handler);

/* Category */
app.MapMethods(CategoryPost.Template, CategoryPost.Methods, CategoryPost.Handler);
app.MapMethods(CategoryGetAll.Template, CategoryGetAll.Methods, CategoryGetAll.Handler);
app.MapMethods(CategoryGetById.Template, CategoryGetById.Methods, CategoryGetById.Handler);
app.MapMethods(CategoryPut.Template, CategoryPut.Methods, CategoryPut.Handler);
app.MapMethods(CategoryDelete.Template, CategoryDelete.Methods, CategoryDelete.Handler);

/* Costumer */
app.MapMethods(CostumerPost.Template, CostumerPost.Methods, CostumerPost.Handler);
app.MapMethods(CostumerGet.Template, CostumerGet.Methods, CostumerGet.Handler);

/* Employee */
app.MapMethods(EmployeePost.Template, EmployeePost.Methods, EmployeePost.Handler);
app.MapMethods(EmployeeGetAll.Template, EmployeeGetAll.Methods, EmployeeGetAll.Handler);

/* Order */
app.MapMethods(OrderPost.Template, OrderPost.Methods, OrderPost.Handler);
app.MapMethods(OrdeGet.Template, OrdeGet.Methods, OrdeGet.Handler);

/* Auth */
app.MapMethods(AuthPost.Template, AuthPost.Methods, AuthPost.Handler);

app.UseExceptionHandler("/error");
app.Map("/error", (HttpContext http) =>
{
    var error = http.Features?.Get<IExceptionHandlerFeature>()?.Error;

    return error switch
    {
        SqlException => Results.Problem(title: "Database is down", statusCode: 500),
        BadHttpRequestException => Results.Problem(title: "Error converting data to another type. Check all information sent", statusCode: 400),
        _ => Results.Problem(title: "An error occurred", statusCode: 500)
    };
});


#endregion Routes



app.Run();
