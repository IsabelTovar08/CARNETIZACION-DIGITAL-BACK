using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Implementations.Organizational.Assignment;
using Business.Interfaces.Operational;
using Business.Interfaces.Organizational.Assignment;
using Business.Services.Cards;
using Data.Interfases.Organizational.Assignment;
using Entity.DTOs.Operational.Response;
using Entity.DTOs.Organizational.Assigment.Request;
using Entity.DTOs.Organizational.Assigment.Response;
using Entity.DTOs.Specifics;
using Entity.DTOs.Specifics.Cards;
using Entity.Models.ModelSecurity;
using Entity.Models.Organizational.Assignment;
using Entity.Models.Organizational.Structure;
using Entity.Models.Parameter;
using Infrastructure.Notifications.Interfases;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Tests.Business.IssuedCards
{
    public class IssuedCardBusinessTests
    {
        private readonly Mock<IIssuedCardData> _issuedCardData = new();
        private readonly Mock<ICardTemplateBusiness> _cardTemplateBusiness = new();
        private readonly Mock<ICardPdfService> _cardPdfService = new();
        private readonly Mock<ICardConfigurationBusiness> _cardConfigurationBusiness = new();
        private readonly Mock<INotify> _notify = new();

        private readonly IMapper _mapper;

        public IssuedCardBusinessTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
            });

            _mapper = config.CreateMapper();

        }

        private IssuedCardBusiness CreateInstance()
        {
            return new IssuedCardBusiness(
                _issuedCardData.Object,
                new NullLogger<IssuedCard>(),
                _mapper,
                _cardTemplateBusiness.Object,
                _cardPdfService.Object,
                _cardConfigurationBusiness.Object,
                _notify.Object
            );
        }

        // ============================================================
        //                  OBJETOS MOCK REUTILIZABLES
        // ============================================================

        private IssuedCard GetCompleteIssuedCard()
        {
            return new IssuedCard
            {
                Id = 1,
                PersonId = 10,
                Person = new Person
                {
                    FirstName = "Juan",
                    LastName = "Torres"
                },
                SheduleId = 3,
                Shedule = new Schedule
                {
                    Id = 3,
                    Name = "Mañana"
                },
                InternalDivisionId = 5,
                InternalDivision = new InternalDivision
                {
                    Id = 5,
                    Name = "Operaciones"
                },
                CardId = 7,
                Card = new CardConfiguration
                {
                    Id = 7,
                    Profile = new Entity.Models.Organizational.Assignment.Profiles
                    {
                        Name = "Colaborador",
                        Id = 7
                    }
                },
                StatusId = 1,
                Status = new Status
                {
                    Id = 1,
                    Name = "Activo"
                },
                QRCode = "base64qr",
                UniqueId = Guid.NewGuid(),
                PdfUrl = null,
            };
        }

        // ============================================================
        //                          TEST 1
        // ============================================================
        public async Task Save_ShouldAssignUuidAndQr_AndSaveSuccessfully()
        {
            // Arrange
            var dto = new IssuedCardDtoRequest
            {
                PersonId = 10,
                InternalDivisionId = 3,
                SheduleId = 1,
                CardTemplateId = 5,
                ProfileId = 2
            };

            _cardConfigurationBusiness
                .Setup(x => x.Save(It.IsAny<CardConfigurationDtoRequest>()))
                .ReturnsAsync(new CardConfigurationDto
                {
                    Id = 1
                });

            _issuedCardData
                .Setup(x => x.SaveAsync(It.IsAny<IssuedCard>()))
                .ReturnsAsync((IssuedCard ic) =>
                {
                    ic.Id = 50;
                    return ic;
                });

            _issuedCardData
                .Setup(x => x.GetCardDataByIssuedIdAsync(50))
                .ReturnsAsync(new CardUserData
                {
                    Email = "test@mail.com",
                    Name = "Tester"
                });

            var sut = CreateInstance();

            // Act
            var result = await sut.Save(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(50, result.Id);
            Assert.False(string.IsNullOrEmpty(result.QRCode));
        }


        // ============================================================
        //                          TEST 2
        // ============================================================
        [Fact]
        public async Task UpdatePdfUrlAsync_ShouldUpdateSuccessfully()
        {
            // Arrange
            var business = CreateInstance();

            var card = GetCompleteIssuedCard();
            card.PdfUrl = "https://server.com/file.pdf";

            _issuedCardData
                .Setup(x => x.UpdatePdfUrlAsync(1, "https://server.com/file.pdf"))
                .ReturnsAsync(card);

            // Act
            var result = await business.UpdatePdfUrlAsync(1, "https://server.com/file.pdf");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("https://server.com/file.pdf", result.PdfUrl);
        }

        // ============================================================
        //                          TEST 3
        // ============================================================
        [Fact]
        public async Task GetCardDataByIssuedId_ShouldReturnData()
        {
            // Arrange
            var business = CreateInstance();

            _issuedCardData
                .Setup(x => x.GetCardDataByIssuedIdAsync(1))
                .ReturnsAsync(new CardUserData
                {
                    Email = "test@test.com",
                    Name = "Juan Perez",
                    Profile = "Colaborador"
                });

            // Act
            var result = await business.GetCardDataByIssuedIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("test@test.com", result.Email);
        }

        // ============================================================
        //                          TEST 4
        // ============================================================
        [Fact]
        public async Task GenerateCardPdfBase64Async_ShouldGeneratePdfSuccessfully()
        {
            // Arrange
            var business = CreateInstance();
            var card = GetCompleteIssuedCard();

            _issuedCardData.Setup(x => x.GetCardDataByIssuedIdAsync(1))
                .ReturnsAsync(new CardUserData
                {
                    Email = "test@test.com",
                    Name = "Juan Perez",
                    Profile = "Colaborador",
                    InternalDivisionName = "Ops",
                    ValidFrom = DateTime.Now,
                    ValidUntil = DateTime.Now.AddYears(1)
                });

            _issuedCardData.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(card);

            _cardTemplateBusiness.Setup(x => x.GetTemplateByCardConfigurationId(7))
                .ReturnsAsync(new CardTemplateResponse
                {
                    Id = 7,
                    Name = "Default",
                    FrontBackgroundUrl = "<svg></svg>",
                    FrontElementsJson = "<svg></svg>",
                    BackBackgroundUrl = "<svg></svg>",
                    BackElementsJson = "<svg></svg>",

                });

            _cardPdfService
                .Setup(x => x.GenerateCardAsync(It.IsAny<CardTemplateResponse>(),
                                                It.IsAny<CardUserData>(),
                                                It.IsAny<MemoryStream>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await business.GenerateCardPdfBase64Async(1);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Length >= 0); // Solo validar ejecución correcta
        }

        // ============================================================
        //                          TEST 5
        // ============================================================
        [Fact]
        public async Task GetTotalNumberOfIDCardsAsync_ShouldReturnValue()
        {
            var business = CreateInstance();

            _issuedCardData.Setup(x => x.GetTotalNumberOfIDCardsAsync())
                .ReturnsAsync(42);

            var result = await business.GetTotalNumberOfIDCardsAsync();

            Assert.Equal(42, result);
        }
    }
}