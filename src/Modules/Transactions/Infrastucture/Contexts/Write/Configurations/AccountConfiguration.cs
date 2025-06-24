using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLedger.Common.Infrastructure.Configurators;
using SmartLedger.Modules.Transactions.Domain.Aggregates;

namespace SmartLedger.Modules.Transactions.Infrastructure.Contexts.Write.Configurations;

public sealed class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.Property(a => a.Id)
            .IsGuid();

        builder.Property(a => a.Name);
    }
}