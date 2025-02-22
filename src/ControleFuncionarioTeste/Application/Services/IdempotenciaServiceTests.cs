using FluentAssertions;
using Moq;
using Questao5.Application.Services.Interfaces;
using Questao5.Domain.Entities;
using Xunit;

namespace Questao5.Application.Services
{
    public class IdempotenciaServiceTests
    {
        private readonly Mock<IIdempotenciaRepository> _mockRepository;
        private readonly IIdempotenciaService _idempotenciaService;

        public IdempotenciaServiceTests()
        {
            _mockRepository = new Mock<IIdempotenciaRepository>();
            _idempotenciaService = new IdempotenciaService(_mockRepository.Object);
        }
        [Fact]
        public void AdicionarRequest_DeveChamarAdicionarRequestDoRepositorio()
        {
            // Arrange
            var request = "request123";
            _mockRepository.Setup(r => r.AdicionarRequest(It.IsAny<string>())).Returns("chave123");

            // Act
            var result = _idempotenciaService.AdicionarRequest(request);

            // Assert
            result.Should().Be("chave123");
            _mockRepository.Verify(r => r.AdicionarRequest(request), Times.Once);
        }

        [Fact]
        public void AdicionarResponse_DeveChamarAdicionarResponseDoRepositorio()
        {
            // Arrange
            var response = "response123";
            var chave = "chave123";

            // Act
            _idempotenciaService.AdicionarResponse(response, chave);

            // Assert
            _mockRepository.Verify(r => r.AdicionarResponse(response, chave), Times.Once); 
        }

        [Fact]
        public void ObterTodos_DeveRetornarListaDeIdempotencia()
        {
            // Arrange
            var idempotencias = new List<Idempotencia>
            {
                new Idempotencia { chave_idempotencia = "chave1", requisicao = "request1", resultado = "response1" },
                new Idempotencia { chave_idempotencia = "chave2", requisicao = "request2", resultado = "response2" }
            };
            _mockRepository.Setup(r => r.ObterTodos()).Returns(idempotencias);

            // Act
            var result = _idempotenciaService.ObterTodos();

            // Assert
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(idempotencias);
        }
    }
}
