using Crypto.Application.Modules.Crypto.Queries.FetchAll;

namespace Crypto.GraphQL.Configuration
{
    public class FetchAllResponseDtoType : ObjectType<FetchAllResponseDto>
    {
        protected override void Configure(IObjectTypeDescriptor<FetchAllResponseDto> descriptor)
        {
            descriptor.Field(i => i.Name).Description("Crypto name");
            descriptor.Field(i => i.Symbol).Description("Crypto symbol");
            descriptor.Field(i => i.Price).Description("Crypto price");
            descriptor.Field(i => i.Description).Description("Crypto description");
            descriptor.Field(i => i.Website).Description("Crypto website");
            descriptor.Field(i => i.SourceCode).Description("Crypto github profile");
            descriptor.Field(i => i.Logo).Description("Crypto logo");

            descriptor.Ignore(i => i.Created);
        }
    }
}
