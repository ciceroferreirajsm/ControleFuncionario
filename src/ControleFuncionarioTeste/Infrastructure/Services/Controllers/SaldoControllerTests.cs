using Moq;
using System;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Services.Interfaces;
using Questao5.Domain.Entities;
using MovimentacaoAPI.Controllers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Questao5.Tests
{
    public class SaldoControllerTests
    {
        private readonly Mock<IContaCorrenteService> _mockContaCorrenteService;
        private readonly Mock<IMovimentoService> _mockMovimentoService;
        private readonly SaldoController _controller;

        public SaldoControllerTests()
        {
            _mockContaCorrenteService = new Mock<IContaCorrenteService>();
            _mockMovimentoService = new Mock<IMovimentoService>();
            _controller = new SaldoController(_mockContaCorrenteService.Object, _mockMovimentoService.Object);
        }

        [Fact]
        public void ConsultarSaldo_ContaNaoExistente_ReturnsBadRequest()
        {
            // Arrange
            string contaCorrenteId = "12345";
            _mockContaCorrenteService.Setup(s => s.ObterPorId(contaCorrenteId)).Returns((ContaCorrente)null); // Simulate account not found

            // Act
            var result = _controller.ConsultarSaldo(contaCorrenteId);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
            var response = badRequestResult.Value as ContaCorrenteBadRequestResponse;
            Assert.Equal("Conta corrente não cadastrada.", response.mensagem);
            Assert.Equal("INVALID_ACCOUNT", response.tipo);
        }

        [Fact]
        public void ConsultarSaldo_ContaInativa_ReturnsBadRequest()
        {
            // Arrange
            string contaCorrenteId = "12345";
            var conta = new ContaCorrente { numero = contaCorrenteId, nome = "John Doe", ativo = false };
            _mockContaCorrenteService.Setup(s => s.ObterPorId(contaCorrenteId)).Returns(conta); // Simulate inactive account

            // Act
            var result = _controller.ConsultarSaldo(contaCorrenteId);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
            var response = badRequestResult.Value as ContaCorrenteBadRequestResponse;
            Assert.Equal("Conta corrente inativa.", response.mensagem);
            Assert.Equal("INACTIVE_ACCOUNT", response.tipo);
        }

        [Fact]
        public void ConsultarSaldo_Success_ReturnsOkWithBalance()
        {
            // Arrange
            string contaCorrenteId = "12345";
            var conta = new ContaCorrente { numero = contaCorrenteId, nome = "John Doe", ativo = true };
            decimal saldo = 1500.50m;
            _mockContaCorrenteService.Setup(s => s.ObterPorId(contaCorrenteId)).Returns(conta);
            _mockMovimentoService.Setup(s => s.ObterPorConta(contaCorrenteId)).Returns(saldo); 

            // Act
            var result = _controller.ConsultarSaldo(contaCorrenteId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            var response = okResult.Value as ContaCorrenteResponse;
            Assert.NotNull(response);
            Assert.Equal(conta.numero, response.NumeroConta);
            Assert.Equal(conta.nome, response.NomeTitular);
            Assert.Equal(saldo, response.SaldoAtual);
            Assert.Equal(DateTime.Today.Date, response.DataHoraResposta.Date);
        }
    }
}
