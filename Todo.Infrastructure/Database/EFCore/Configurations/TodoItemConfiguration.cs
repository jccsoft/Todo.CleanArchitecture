using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Todo.Infrastructure.Database.EFCore.Configurations;

internal sealed class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        builder.ToTable("todoitems");

        builder.HasKey(todoItem => todoItem.Id);

        builder.OwnsOne(todoItem => todoItem.Title,
                        navigationBuilder =>
                        {
                            navigationBuilder.Property(title => title.Value)
                                             .HasColumnName("Title");
                        });

    }
}
