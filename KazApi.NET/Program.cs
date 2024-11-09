

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//builder.Services.AddDbContext<TodoContext>(
//	opt => opt.UseInMemoryDatabase("TodoList")
//);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS�I���W���ݒ�
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()  // ���ׂẴI���W������� CORS �v��������
                   .AllowAnyMethod()  // ���ׂĂ� HTTP ���\�b�h������
                   .AllowAnyHeader(); // ���ׂĂ̍쐬�җv���w�b�_�[������
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

// CORS �~�h���E�F�A��L���ɂ���
app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
