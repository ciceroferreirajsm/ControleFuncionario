using Moq;
using Questao5.Application.Services.Interfaces;
using Questao5.Domain.Entities;
using Xunit;

namespace Questao5.Application.Services
{
    public class ContaCorrenteServiceTests
    {
        private readonly Mock<IContaCorrenteRepository> _mockContaCorrenteRepository;
        private readonly IContaCorrenteService _contaCorrenteService;

        public ContaCorrenteServiceTests()
        {
            _mockContaCorrenteRepository = new Mock<IContaCorrenteRepository>();
            _contaCorrenteService = new ContaCorrenteService(_mockContaCorrenteRepository.Object);
        }

        [Fact]
        public void ObterContaPorId_ContaExistente_RetornaConta()
        {
            // Arrange
            var contaCorrenteId = "123";
            var contaCorrente = new ContaCorrente
            {
                idcontacorrente = contaCorrenteId,
                ativo = true,
                saldo = 1000
            };

            _mockContaCorrenteRepository
                .Setup(repo => repo.ObterPorId(contaCorrenteId))
                .Returns(contaCorrente);

            // Act
            var result = _contaCorrenteService.ObterPorId(contaCorrenteId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(contaCorrenteId, result?.idcontacorrente);
        }

        [Fact]
        public void ObterContaPorId_ContaNaoExistente_RetornaNull()
        {
            // Arrange
            var contaCorrenteId = "123";

            _mockContaCorrenteRepository
                .Setup(repo => repo.ObterPorId(contaCorrenteId))
                .Returns((ContaCorrente?)null);

            // Act
            var result = _contaCorrenteService.ObterPorId(contaCorrenteId);

            // Assert
            Assert.Null(result);
        }
    }
}
