

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

[EntityTypeConfiguration(typeof(PuzzleEntityConfiguration))]
public class Puzzle
{
  public int Id { get; set; }
  public required string Initial { get; set; }
  public string? Solution { get; set; }
  public decimal? Difficulty { get; set; }
}

public class PuzzleEntityConfiguration : IEntityTypeConfiguration<Puzzle>
{
  public void Configure(EntityTypeBuilder<Puzzle> builder)
  {
    builder.ToTable("Puzzles");
    builder.HasKey(x => x.Id);
    builder.Property(x => x.Id).HasColumnName("Id").ValueGeneratedOnAdd();
    builder.Property(x => x.Initial).HasColumnName("Initial");
    builder.Property(x => x.Solution).HasColumnName("Solution").IsRequired(false);
    builder.Property(x => x.Difficulty).HasColumnName("Difficulty").IsRequired(false);
    builder.HasIndex(x => x.Initial).IsUnique(true);
  }
}