using Xunit;
using Moq;
using AutoMapper;
using ControleFuncionario.Application.Repository.Interfaces;
using ControleFuncionario.Application.Services;
using ControleFuncionario.Domain.Entities;
using ControleFuncionario.Model;

public class LoginServiceTests
{
    private readonly Mock<ILoginRepository> _loginRepositoryMock;
    private readonly IMapper _mapper;
    private readonly LoginService _loginService;

    public LoginServiceTests()
    {
        _loginRepositoryMock = new Mock<ILoginRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<LoginDTO, Login>();
            cfg.CreateMap<Login, LoginDTO>();
        });
        _mapper = config.CreateMapper();

        _loginService = new LoginService(_loginRepositoryMock.Object, _mapper);
    }

    [Fact]
    public void Adicionar_DeveChamarRepositorioEHashSenha()
    {
        // Arrange
        var loginDTO = new LoginDTO { Email = "teste@email.com", Nome = "Teste", Senha = "1234" };
        _loginRepositoryMock.Setup(repo => repo.Adicionar(It.IsAny<Login>())).Returns(1);

        // Act
        var result = _loginService.Adicionar(loginDTO);

        // Assert
        _loginRepositoryMock.Verify(repo => repo.Adicionar(It.Is<Login>(l => l.Email == loginDTO.Email)), Times.Once);
        Assert.Equal(1, result);
    }

    [Fact]
    public void Logar_DeveRetornarLoginDTOQuandoCredenciaisCorretas()
    {
        // Arrange
        var senha = "1234";
        var loginDTO = new LoginDTO { Email = "teste@email.com", Senha = senha };
        var login = new Login { Email = "teste@email.com", Senha = BCrypt.Net.BCrypt.HashPassword(senha) };

        _loginRepositoryMock.Setup(repo => repo.ObterPorEmail(loginDTO.Email)).Returns(login);

        // Act
        var result = _loginService.Logar(loginDTO);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(loginDTO.Email, result.Email);
    }

    [Fact]
    public void Logar_DeveRetornarNullQuandoUsuarioNaoEncontrado()
    {
        // Arrange
        var loginDTO = new LoginDTO { Email = "naoexiste@email.com", Senha = "1234" };
        _loginRepositoryMock.Setup(repo => repo.ObterPorEmail(loginDTO.Email)).Returns((Login)null);

        // Act
        var result = _loginService.Logar(loginDTO);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Logar_DeveRetornarNullQuandoSenhaIncorreta()
    {
        // Arrange
        var loginDTO = new LoginDTO { Email = "teste@email.com", Senha = "senhaerrada" };
        var login = new Login { Email = "teste@email.com", Senha = BCrypt.Net.BCrypt.HashPassword("senhaCorreta") };

        _loginRepositoryMock.Setup(repo => repo.ObterPorEmail(loginDTO.Email)).Returns(login);

        // Act
        var result = _loginService.Logar(loginDTO);

        // Assert
        Assert.Null(result);
    }
}
