namespace Cloudmoree;

// You’re implementing a user roles registry that customers fully control for their organizations.
// Each role can have capabilities and include other roles, inheriting their capabilities.
// Example of roles for SillyPenguins LLC organization:
// Seagull
// capabilities: { "view_posts" }
// Penguin - everything from Seagull role and
// capabilities: { "create_posts", "comment_posts" }
// GrandPenguin - everything from Penguin role and
// capabilities: { "create_video_posts" }
// Moderator - everything from Seagull role and
// capabilities: { "comment_posts", "modify_comments", "mod_badge" }
// KingPenguin - everything from GrandPenguin and Moderator roles and
// capabilities: { "modify_posts", "crown_badge" }
// It should be possible to get all the capabilities of a role at once. 
// For example, the Penguin role should allow view_posts, create_posts, 
// comment_posts capabilities. Capabilities added to or removed from a role 
// should affect all roles that directly or indirectly include it.

public interface IRole{
    string Name { get; set; }
    List<string> Capabilities { get; set; }
    List<IRole> IncludedRoles { get; set; }

    void AddCapability(string capability);
    void RemoveCapability(string capability);
    void IncludeRole(IRole role);
    void ExcludeRole(IRole role);
    List<string> GetAllCapabilities();
}

public class Role : IRole{
    public string Name { get; set; }
    public List<string> Capabilities { get; set; } = [];
    public List<IRole> IncludedRoles { get; set; } = [];

    public Role(string name){
        Name = name;
        Capabilities = [];
        IncludedRoles = [];
    }

    public void AddCapability(string capability){
        if (!Capabilities.Contains(capability)){
            Capabilities.Add(capability);
        }
    }

    public void RemoveCapability(string capability){
        Capabilities.Remove(capability);
    }

    public void IncludeRole(IRole role){
        if (!IncludedRoles.Contains(role)){
            IncludedRoles.Add(role);
        }
    }

    public void ExcludeRole(IRole role){
        IncludedRoles.Remove(role);
    }

    public List<string> GetAllCapabilities(){
        var allCapabilities = new List<string>(Capabilities);

        foreach (var role in IncludedRoles){
            allCapabilities.AddRange(role.GetAllCapabilities());
        }

        return allCapabilities.Distinct().ToList();
    }
}

public class Program{
    static void Main(){
        IRole seagull = new Role("Seagull");
        seagull.AddCapability("view_posts");

        IRole penguin = new Role("Penguin");
        penguin.AddCapability("create_posts");
        penguin.AddCapability("comment_posts");
        penguin.IncludeRole(seagull);

        Console.WriteLine($"Seagull capabilities: {string.Join(", ", seagull.GetAllCapabilities())}");
        Console.WriteLine($"Penguin capabilities: {string.Join(", ", penguin.GetAllCapabilities())}");
    }
}
