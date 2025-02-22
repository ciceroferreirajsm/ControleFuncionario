using Xunit;
using Moq;
using AutoMapper;
using ControleFuncionario.Application.Repository.Interfaces;
using ControleFuncionario.Application.Services;
using ControleFuncionario.Application.Services.Interfaces;
using ControleFuncionario.Domain.Entities;
using ControleFuncionario.Model;
using Bogus;

public class FuncionarioServiceTests
{
    private readonly Mock<IFuncionarioRepository> _funcionarioRepositoryMock;
    private readonly Mock<ILoginService> _loginServiceMock;
    private readonly IMapper _mapper;
    private readonly FuncionarioService _funcionarioService;

    public FuncionarioServiceTests()
    {
        _funcionarioRepositoryMock = new Mock<IFuncionarioRepository>();
        _loginServiceMock = new Mock<ILoginService>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<FuncionarioDTO, Funcionario>().ReverseMap();
        });
        _mapper = config.CreateMapper();

        _funcionarioService = new FuncionarioService(
            _funcionarioRepositoryMock.Object,
            _mapper,
            _loginServiceMock.Object
        );
    }

    [Fact]
    public void Adicionar_DeveChamarRepositorioELoginService()
    {
        // Arrange
        var funcionarioDTO = CriarMock();
        var funcionario = _mapper.Map<Funcionario>(funcionarioDTO);
        _funcionarioRepositoryMock.Setup(repo => repo.Adicionar(It.IsAny<Funcionario>())).Returns(1);

        // Act
        var result = _funcionarioService.Adicionar(funcionarioDTO);

        // Assert
        _funcionarioRepositoryMock.Verify(repo => repo.Adicionar(It.IsAny<Funcionario>()), Times.Once);
        _loginServiceMock.Verify(login => login.Adicionar(It.IsAny<LoginDTO>()), Times.Once);
        Assert.Equal(1, result);
    }

    [Fact]
    public void Detalhes_DeveRetornarFuncionarioDTO()
    {
        // Arrange
        var funcionario = new Funcionario { Id = 1, Nome = "Teste", Email = "teste@email.com" };
        _funcionarioRepositoryMock.Setup(repo => repo.Detalhes(1)).Returns(funcionario);

        // Act
        var result = _funcionarioService.Detalhes(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Teste", result.Nome);
        Assert.Equal("teste@email.com", result.Email);
    }

    [Fact]
    public void Listar_DeveRetornarListaDeFuncionarios()
    {
        // Arrange
        var funcionarios = new List<Funcionario> { new Funcionario { Id = 1, Nome = "Teste" } };
        _funcionarioRepositoryMock.Setup(repo => repo.Listar()).Returns(funcionarios);

        // Act
        var result = _funcionarioService.Listar();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Teste", result.First().Nome);
    }

    [Fact]
    public void Atualizar_DeveRetornarTrueQuandoSucesso()
    {
        // Arrange
        var funcionarioDTO = new FuncionarioDTO { id = "1", Nome = "Atualizado" };
        _funcionarioRepositoryMock.Setup(repo => repo.Atualizar(It.IsAny<Funcionario>())).Returns(true);

        // Act
        var result = _funcionarioService.Atualizar(funcionarioDTO);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Deletar_DeveRetornarTrueQuandoSucesso()
    {
        // Arrange
        _funcionarioRepositoryMock.Setup(repo => repo.Deletar(1)).Returns(true);

        // Act
        var result = _funcionarioService.Deletar(1);

        // Assert
        Assert.True(result);
    }

    public static FuncionarioDTO CriarMock()
    {
        var faker = new Faker("pt_BR");

        return new FuncionarioDTO
        {
            id = faker.Random.Int().ToString(),
            Nome = faker.Name.FirstName(),
            Sobrenome = faker.Name.LastName(),
            Email = faker.Internet.Email(),
            Telefone = faker.Phone.PhoneNumber(),
            Cargo = faker.Name.JobTitle(),
            Documento = faker.Random.Replace("###.###.###-##"),
            Gestor = faker.Name.FullName(),
            Senha = faker.Internet.Password(),
            DtNascimento = faker.Date.Past(30, DateTime.Now.AddYears(-18)).ToShortDateString(),
            Ativo = faker.Random.Bool(),
            Permissao = faker.PickRandom("direto", "lider", "funcionario")
        };
    }
}
