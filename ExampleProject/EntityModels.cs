using System.ComponentModel.DataAnnotations.Schema;

namespace ExampleProject;

public abstract class AbstractSkill
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<PlayerToSkill> PlayersWithSkill { get; set; } = null!;
}

public class MartialSkill : AbstractSkill
{
    public bool HasStrike { get; set; }
}

public class MagicSkill : AbstractSkill
{
    public string RunicName { get; set; } = null!;
}

public class Player
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public ICollection<PlayerToSkill> Skills { get; set; } = null!;
}

public class PlayerToSkill
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public Guid PlayerId { get; set; }
    public Player Player { get; set; } = null!;

    public Guid SkillId { get; set; }
    public AbstractSkill Skill { get; set; } = null!;
}
