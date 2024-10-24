var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["ConnectionString:IWantDb"]);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddScoped<QueryAllUsersWithClaimName>();
builder.Services.AddJwtAuthentication(builder.Configuration); /* Authentication */
builder.Services.AddCustomAuthorization(); /* Requires authenticated user by default */

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

app.MapMethods(CategoryPost.Template, CategoryPost.Methods, CategoryPost.Handler);
app.MapMethods(CategoryGetAll.Template, CategoryGetAll.Methods, CategoryGetAll.Handler);
app.MapMethods(CategoryGetById.Template, CategoryGetById.Methods, CategoryGetById.Handler);
app.MapMethods(CategoryPut.Template, CategoryPut.Methods, CategoryPut.Handler);
app.MapMethods(CategoryDelete.Template, CategoryDelete.Methods, CategoryDelete.Handler);
app.MapMethods(EmployeePost.Template, EmployeePost.Methods, EmployeePost.Handler);
app.MapMethods(EmployeeGetAll.Template, EmployeeGetAll.Methods, EmployeeGetAll.Handler);
app.MapMethods(AuthPost.Template, AuthPost.Methods, AuthPost.Handler);

#endregion Routes


app.Run();
