using IWantApp.Database;
using IWantApp.Domain.Products;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["ConnectionString:IWantDb"]);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

#region Routes

app.MapMethods(CategoryPost.Template, CategoryPost.Methods, CategoryPost.Handler);
app.MapMethods(CategoryGetAll.Template, CategoryGetAll.Methods, CategoryGetAll.Handler);
app.MapMethods(CategoryGetById.Template, CategoryGetById.Methods, CategoryGetById.Handler);
app.MapMethods(CategoryPut.Template, CategoryPut.Methods, CategoryPut.Handler);
app.MapMethods(CategoryDelete.Template, CategoryDelete.Methods, CategoryDelete.Handler);

#endregion Routes


app.Run();
