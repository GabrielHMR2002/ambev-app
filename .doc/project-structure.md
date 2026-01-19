[Back to README](../README.md)

## Project Structure

The project should be structured as follows:

```
Ambev.DeveloperEvaluation
├── Application
│   ├── Auth
│   │   └── AuthenticateUser
│   │       ├── AuthenticateUserCommand.cs
│   │       ├── AuthenticateUserHandler.cs
│   │       ├── AuthenticateUserProfile.cs
│   │       ├── AuthenticateUserResult.cs
│   │       └── AuthenticateUserValidator.cs
│   ├── Sales
│   │   ├── CancelSale
│   │   │   ├── CancelSaleCommand.cs
│   │   │   ├── CancelSaleHandler.cs
│   │   │   ├── CancelSaleProfile.cs
│   │   │   ├── CancelSaleResult.cs
│   │   │   └── CancelSaleValidator.cs
│   │   ├── CancelSaleItem
│   │   │   ├── CancelSaleItemCommand.cs
│   │   │   ├── CancelSaleItemHandler.cs
│   │   │   ├── CancelSaleItemProfile.cs
│   │   │   ├── CancelSaleItemResult.cs
│   │   │   └── CancelSaleItemValidator.cs
│   │   ├── CreateSale
│   │   │   ├── CreateSaleCommand.cs
│   │   │   ├── CreateSaleHandler.cs
│   │   │   ├── CreateSaleProfile.cs
│   │   │   ├── CreateSaleResult.cs
│   │   │   └── CreateSaleValidator.cs
│   │   ├── DeleteSale
│   │   │   ├── DeleteSaleCommand.cs
│   │   │   ├── DeleteSaleHandler.cs
│   │   │   ├── DeleteSaleProfile.cs
│   │   │   ├── DeleteSaleResult.cs
│   │   │   └── DeleteSaleValidator.cs
│   │   ├── GetAllSales
│   │   │   ├── GetAllSalesCommand.cs
│   │   │   ├── GetAllSalesHandler.cs
│   │   │   ├── GetAllSalesProfile.cs
│   │   │   ├── GetAllSalesResult.cs
│   │   │   └── GetAllSalesValidator.cs
│   │   ├── GetSale
│   │   │   ├── GetSaleCommand.cs
│   │   │   ├── GetSaleHandler.cs
│   │   │   ├── GetSaleProfile.cs
│   │   │   ├── GetSaleResult.cs
│   │   │   └── GetSaleValidator.cs
│   │   └── UpdateSale
│   │       ├── UpdateSaleCommand.cs
│   │       ├── UpdateSaleHandler.cs
│   │       ├── UpdateSaleProfile.cs
│   │       ├── UpdateSaleResult.cs
│   │       └── UpdateSaleValidator.cs
│   ├── Users
│   │   ├── CreateUser
│   │   │   ├── CreateUserCommand.cs
│   │   │   ├── CreateUserHandler.cs
│   │   │   ├── CreateUserProfile.cs
│   │   │   ├── CreateUserResult.cs
│   │   │   └── CreateUserValidator.cs
│   │   ├── DeleteUser
│   │   │   ├── DeleteUserCommand.cs
│   │   │   ├── DeleteUserHandler.cs
│   │   │   ├── DeleteUserResponse.cs
│   │   │   └── DeleteUserValidator.cs
│   │   └── GetUser
│   │       ├── GetUserCommand.cs
│   │       ├── GetUserHandler.cs
│   │       ├── GetUserProfile.cs
│   │       ├── GetUserResult.cs
│   │       └── GetUserValidator.cs
│   └── ApplicationLayer.cs
├── Common
│   ├── HealthChecks
│   │   └── HealthChecksExtension.cs
│   ├── Logging
│   │   └── LoggingExtension.cs
│   ├── Security
│   │   ├── AuthenticationExtension.cs
│   │   ├── BCryptPasswordHasher.cs
│   │   ├── IJwtTokenGenerator.cs
│   │   ├── IPasswordHasher.cs
│   │   ├── IUser.cs
│   │   └── JwtTokenGenerator.cs
│   └── Validation
│       ├── ValidationBehavior.cs
│       ├── ValidationErrorDetail.cs
│       ├── ValidationResult.cs
│       └── Validator.cs
├── Domain
│   ├── Common
│   │   └── BaseEntity.cs
│   ├── Entities
│   │   ├── ItemCancelledEvent.cs
│   │   ├── Sale.cs
│   │   ├── SaleCancelledEvent.cs
│   │   ├── SaleItem.cs
│   │   └── User.cs
│   ├── Enums
│   │   ├── UserRole.cs
│   │   └── UserStatus.cs
│   ├── Events
│   │   ├── SaleCreatedEvent.cs
│   │   ├── SaleModifiedEvent.cs
│   │   └── UserRegisteredEvent.cs
│   ├── Exceptions
│   │   └── DomainException.cs
│   ├── Repositories
│   │   ├── ISaleRepository.cs
│   │   └── IUserRepository.cs
│   ├── Services
│   │   └── IUserService.cs
│   ├── Specifications
│   │   ├── ActiveUserSpecification.cs
│   │   └── ISpecification.cs
│   └── Validation
│       ├── EmailValidator.cs
│       ├── PasswordValidator.cs
│       ├── PhoneValidator.cs
│       ├── SaleItemValidator.cs
│       ├── SaleValidator.cs
│       └── UserValidator.cs
├── IoC
│   ├── ModuleInitializers
│   │   ├── ApplicationModuleInitializer.cs
│   │   ├── InfrastructureModuleInitializer.cs
│   │   └── WebApiModuleInitializer.cs
│   ├── DependencyResolver.cs
│   └── IModuleInitializer.cs
├── MessageBroker
│   ├── Configuration
│   │   ├── MessageBrokerConfiguration.cs
│   │   ├── QueueConfiguration.cs
│   │   └── RabbitMQSettings.cs
│   ├── Initialization
│   │   └── RabbitMQInitializer.cs
│   ├── Interfaces
│   │   └── IMessagePublisher.cs
│   ├── Messages
│   │   ├── ItemCancelledMessage.cs
│   │   ├── SaleCancelledMessage.cs
│   │   ├── SaleCreatedMessage.cs
│   │   ├── SaleItemMessage.cs
│   │   └── SaleModifiedMessage.cs
│   └── Publishers
│       └── RabbitMQPublisher.cs
├── ORM
│   ├── Mapping
│   │   ├── SaleConfiguration.cs
│   │   ├── SaleItemConfiguration.cs
│   │   └── UserConfiguration.cs
│   ├── Migrations
│   │   ├── 20241014011203_InitialMigrations.cs
│   │   ├── 20241014011203_InitialMigrations.Designer.cs
│   │   ├── 20260118183903_InitialCreate.cs
│   │   ├── 20260118183903_InitialCreate.Designer.cs
│   │   └── DefaultContextModelSnapshot.cs
│   ├── Repositories
│   │   ├── SaleRepository.cs
│   │   └── UserRepository.cs
│   └── DefaultContext.cs
├── WebApi
│   ├── Common
│   │   ├── ApiResponse.cs
│   │   ├── ApiResponseWithData.cs
│   │   ├── BaseController.cs
│   │   ├── PaginatedList.cs
│   │   └── PaginatedResponse.cs
│   ├── Features
│   │   ├── Auth
│   │   │   └── AuthenticateUserFeature
│   │   │       ├── AuthenticateUserProfile.cs
│   │   │       ├── AuthenticateUserRequest.cs
│   │   │       ├── AuthenticateUserRequestValidator.cs
│   │   │       └── AuthenticateUserResponse.cs
│   │   │
│   │   ├── Sales
│   │   │   ├── CancelSale
│   │   │   │   ├── CancelSaleProfile.cs
│   │   │   │   ├── CancelSaleRequest.cs
│   │   │   │   ├── CancelSaleRequestValidator.cs
│   │   │   │   └── CancelSaleResponse.cs
│   │   │   ├── CancelSaleItem
│   │   │   │   ├── CancelSaleItemProfile.cs
│   │   │   │   ├── CancelSaleItemRequest.cs
│   │   │   │   ├── CancelSaleItemRequestValidator.cs
│   │   │   │   └── CancelSaleItemResponse.cs
│   │   │   ├── CreateSale
│   │   │   │   ├── CreateSaleProfile.cs
│   │   │   │   ├── CreateSaleRequest.cs
│   │   │   │   ├── CreateSaleRequestValidator.cs
│   │   │   │   └── CreateSaleResponse.cs
│   │   │   ├── DeleteSale
│   │   │   │   ├── DeleteSaleProfile.cs
│   │   │   │   ├── DeleteSaleRequest.cs
│   │   │   │   ├── DeleteSaleRequestValidator.cs
│   │   │   │   └── DeleteSaleResponse.cs
│   │   │   ├── GetAllSales
│   │   │   │   ├── GetAllSalesProfile.cs
│   │   │   │   ├── GetAllSalesRequest.cs
│   │   │   │   ├── GetAllSalesRequestValidator.cs
│   │   │   │   └── GetAllSalesResponse.cs
│   │   │   ├── GetSale
│   │   │   │   ├── GetSaleProfile.cs
│   │   │   │   ├── GetSaleRequest.cs
│   │   │   │   ├── GetSaleRequestValidator.cs
│   │   │   │   └── GetSaleResponse.cs
│   │   │   └── UpdateSale
│   │   │       ├── UpdateSaleProfile.cs
│   │   │       ├── UpdateSaleRequest.cs
│   │   │       ├── UpdateSaleRequestValidator.cs
│   │   │       └── UpdateSaleResponse.cs
│   │   └── Users
│   │       ├── CreateUser
│   │       │   ├── CreateUserProfile.cs
│   │       │   ├── CreateUserRequest.cs
│   │       │   ├── CreateUserRequestValidator.cs
│   │       │   └── CreateUserResponse.cs
│   │       ├── DeleteUser
│   │       │   ├── DeleteUserProfile.cs
│   │       │   ├── DeleteUserRequest.cs
│   │       │   ├── DeleteUserRequestValidator.cs
│   │       │   └── DeleteUserResponse.cs
│   │       └── GetUser
│   │           ├── GetUserProfile.cs
│   │           ├── GetUserRequest.cs
│   │           ├── GetUserRequestValidator.cs
│   │           └── GetUserResponse.cs
│   ├── Mappings
│   │   └── CreateUserRequestProfile.cs
│   ├── Middleware
│   │   └── ValidationExceptionMiddleware.cs
│   └── Program.cs
└── tests
    ├── Ambev.DeveloperEvaluation.Functional
    ├── Ambev.DeveloperEvaluation.Integration
    └── Ambev.DeveloperEvaluation.Unit
        ├── Application
        │   └── TestData
        │       ├── CancelSaleHandlerTestData.cs
        │       ├── CancelSaleItemHandlerTestData.cs
        │       ├── CreateSaleHandlerTestData.cs
        │       ├── CreateUserHandlerTestData.cs
        │       └── UpdateSaleHandlerTestData.cs
        ├── Application
        │   └── CreateUserHandlerTests.cs
        ├── Domain
        │   ├── Entities
        │   │   ├── SaleItemTests.cs
        │   │   ├── SaleTests.cs
        │   │   ├── UserTests.cs
        │   │   └── TestData
        │   │       └── UserTestData.cs
        │   ├── Events
        │   │   ├── ItemCancelledEventTests.cs
        │   │   ├── SaleCancelledEventTests.cs
        │   │   ├── SaleCreatedEventTests.cs
        │   │   └── SaleModifiedEventTests.cs
        │   ├── Specifications
        │   │   ├── ActiveUserSpecificationTests.cs
        │   │   └── TestData
        │   │       └── ActiveUserSpecificationTestData.cs
        │   └── Validation
        │       ├── EmailValidatorTests.cs
        │       ├── PasswordValidatorTests.cs
        │       ├── PhoneValidatorTests.cs
        │       ├── SaleItemValidatorTests.cs
        │       ├── SaleValidatorTests.cs
        │       └── UserValidatorTests.cs
        └── Sales
            ├── CancelSaleHandlerTests.cs
            ├── CancelSaleItemHandlerTests.cs
            ├── CreateSaleHandlerTests.cs
            ├── DeleteSaleHandlerTests.cs
            ├── GetAllSalesHandlerTests.cs
            ├── GetSaleHandlerTests.cs
            └── UpdateSaleHandlerTests.cs

```
