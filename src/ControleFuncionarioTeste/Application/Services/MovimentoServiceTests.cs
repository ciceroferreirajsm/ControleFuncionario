using FluentAssertions;
using Moq;
using Questao5.Application.Services.Interfaces;
using Questao5.Domain.Entities;
using Xunit;

namespace Questao5.Application.Services
{
    public class MovimentoServiceTests
    {
        private readonly Mock<IMovimentoRepository> _mockRepository;
        private readonly IMovimentoService _movimentoService;

        public MovimentoServiceTests()
        {
            _mockRepository = new Mock<IMovimentoRepository>();
            _movimentoService = new MovimentoService(_mockRepository.Object);
        }

        [Fact]
        public void Adicionar_DeveChamarAdicionarDoRepositorio()
        {
            // Arrange
            var movimento = new Movimento { tipomovimento = "C", valor = 100.00M, idcontacorrente = "12345" };
            _mockRepository.Setup(r => r.Adicionar(It.IsAny<Movimento>())).Returns(1);

            // Act
            var result = _movimentoService.Adicionar(movimento);

            // Assert
            result.Should().Be(1);
            _mockRepository.Verify(r => r.Adicionar(movimento), Times.Once);
        }

        [Fact]
        public void ObterPorConta_DeveCalcularSaldoCorretamente_QuandoMovimentosExistem()
        {
            // Arrange
            var movimentos = new List<Movimento>
            {
                new Movimento { tipomovimento = "C", valor = 100.00M, idcontacorrente = "12345" }, // Crédito
                new Movimento { tipomovimento = "D", valor = 50.00M, idcontacorrente = "12345" }  // Débito
            };

            _mockRepository.Setup(r => r.ObterPorConta(It.IsAny<string>())).Returns(movimentos);

            // Act
            var saldo = _movimentoService.ObterPorConta("12345");

            // Assert
            saldo.Should().Be(50.00M);
        }

        [Fact]
        public void ObterPorConta_DeveRetornarSaldoZero_QuandoNaoExistemMovimentos()
        {
            // Arrange
            _mockRepository.Setup(r => r.ObterPorConta(It.IsAny<string>())).Returns(new List<Movimento>());

            // Act
            var saldo = _movimentoService.ObterPorConta("12345");

            // Assert
            saldo.Should().Be(0.00M);
        }

        [Fact]
        public void ObterPorConta_DeveCalcularSaldoCorretamente_QuandoExistemVariosCreditosEDebitos()
        {
            // Arrange
            var movimentos = new List<Movimento>
            {
                new Movimento { tipomovimento = "C", valor = 100.00M, idcontacorrente = "12345" }, // Crédito
                new Movimento { tipomovimento = "D", valor = 50.00M, idcontacorrente = "12345" },  // Débito
                new Movimento { tipomovimento = "C", valor = 200.00M, idcontacorrente = "12345" }, // Crédito
                new Movimento { tipomovimento = "D", valor = 30.00M, idcontacorrente = "12345" }   // Débito
            };

            _mockRepository.Setup(r => r.ObterPorConta(It.IsAny<string>())).Returns(movimentos);

            // Act
            var saldo = _movimentoService.ObterPorConta("12345");

            // Assert
            saldo.Should().Be(220.00M);
        }
    }
}

