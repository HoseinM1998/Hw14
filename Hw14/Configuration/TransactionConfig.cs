using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Hw14.Entities;

namespace Hw14.Configuration
{
    public class TransactionConfig : IEntityTypeConfiguration<Transactiion>
    {
        public void Configure(EntityTypeBuilder<Transactiion> builder)
        {
            builder.HasKey(t => t.TransactionId);

            builder.Property(t => t.SourceCardNumber)
                .IsRequired()
                .HasMaxLength(16);

            builder.Property(t => t.DestinationCardNumber)
                .IsRequired()
                .HasMaxLength(16);

            builder.Property(t => t.Amount)
                .IsRequired();

            builder.Property(t => t.TransactionDate)
                .IsRequired();

            builder.Property(t => t.IsSuccessful)
                .IsRequired();

            builder.HasOne(t => t.Card)
                       .WithMany(c => c.Transactions)
                       .HasForeignKey(t => t.SourceCardNumber)
                       .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Card)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.DestinationCardNumber)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
