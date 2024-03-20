using System;
using System.Threading;

// Enum for Pokemon types
enum Type
{
    Grass,
    Fire,
    Water
}

// Enum for movement directions
enum Direction
{
    Forward,
    Left,
    Right
}

// Class representing a Pokemon
class Pokemon
{
    // Properties for Pokemon attributes
    public string Name { get; }  // Name of the Pokemon
    public int Level { get; private set; }  // Level of the Pokemon
    public int MaxHP { get; private set; }  // Maximum HP of the Pokemon
    private int currentHP; // Current HP of the Pokemon (private field)
    public int CurrentHP // Property for current HP with a public set accessor
    {
        get { return currentHP; }
        set { currentHP = value; }
    }
    public int AttackDamage { get; private set; } // Attack damage of the Pokemon
    public Type PokemonType { get; } // Type of the Pokemon
    public Attack[] Attacks { get; } // Array of attacks the Pokemon can use

    // Constructor for creating a new Pokemon
    public Pokemon(string name, int level, Type type, Attack[] attacks)
    {
        Name = name;
        Level = level;
        MaxHP = 50 + (level - 5) * 10; // Calculate maximum HP based on level
        CurrentHP = MaxHP; // Set current HP to maximum HP
        AttackDamage = 10 + (level - 5) * 2; // Calculate attack damage based on level
        PokemonType = type;
        Attacks = attacks;
    }

    // Method for the Pokemon to gain experience points and level up
    public void GainExp(int exp)
    {
        // Level up if enough experience gained
        if (Level < 100 && exp >= 100)
        {
            Level++;
            MaxHP += 10;
            AttackDamage += 2;
            Console.WriteLine($"{Name} leveled up to level {Level}!");
        }
    }

    // Method for the Pokemon to take damage
    public void TakeDamage(int damage)
    {
        CurrentHP -= damage;
        if (CurrentHP < 0)
            CurrentHP = 0;
    }

    // Method to check if the Pokemon has fainted
    public bool IsFainted()
    {
        return CurrentHP == 0;
    }
}

// Class representing an attack
class Attack
{
    // Properties for attack attributes
    public string Name { get; } // Name of the attack
    public int Damage { get; } // Damage of the attack
    public Type AttackType { get; } // Type of the attack

    // Constructor for creating a new attack
    public Attack(string name, int damage, Type type)
    {
        Name = name;
        Damage = damage;
        AttackType = type;
    }
}

// Main program class
class Program
{
    // ANSI escape codes for text color
    const string RedColor = "\u001b[31m"; // Red color code
    const string GreenColor = "\u001b[32m"; // Green color code
    const string ResetColor = "\u001b[0m"; // Reset color code

    // Main method
    static void Main(string[] args)
    {
        // Set console encoding to UTF-8 to support ANSI escape codes
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // Create starter Pokemon
        Attack[] bulbasaurAttacks = { new Attack("Tackle", 10, Type.Grass), new Attack("Vine Whip", 15, Type.Grass) };
        Pokemon bulbasaur = new Pokemon("Bulbasaur", 5, Type.Grass, bulbasaurAttacks);

        Attack[] charmanderAttacks = { new Attack("Scratch", 10, Type.Fire), new Attack("Ember", 15, Type.Fire) };
        Pokemon charmander = new Pokemon("Charmander", 5, Type.Fire, charmanderAttacks);

        Attack[] squirtleAttacks = { new Attack("Tackle", 10, Type.Water), new Attack("Water Gun", 15, Type.Water) };
        Pokemon squirtle = new Pokemon("Squirtle", 5, Type.Water, squirtleAttacks);

        // Welcome message
        Console.WriteLine($"{GreenColor}Welcome to the Pokemon World!{ResetColor}");

        // Player chooses their Pokemon
        Console.WriteLine($"{GreenColor}Choose your Pokemon:{ResetColor}");
        Console.WriteLine($"{GreenColor}1. Bulbasaur{ResetColor}");
        Console.WriteLine($"{GreenColor}2. Charmander{ResetColor}");
        Console.WriteLine($"{GreenColor}3. Squirtle{ResetColor}");

        int playerChoice = GetChoice(1, 3);

        Pokemon playerPokemon = null;
        switch (playerChoice)
        {
            case 1:
                playerPokemon = bulbasaur;
                break;
            case 2:
                playerPokemon = charmander;
                break;
            case 3:
                playerPokemon = squirtle;
                break;
        }

        Console.WriteLine($"{GreenColor}You chose {playerPokemon.Name}!{ResetColor}");

        Console.WriteLine($"{GreenColor}Now, let's start our adventure!{ResetColor}");

        // Start Route 1
        while (true)
        {
            Console.WriteLine($"{GreenColor}You're on Route 1.{ResetColor}");
            Console.WriteLine($"{GreenColor}Which direction would you like to go?{ResetColor}");
            Console.WriteLine($"{GreenColor}1. Forward{ResetColor}");
            Console.WriteLine($"{GreenColor}2. Left{ResetColor}");
            Console.WriteLine($"{GreenColor}3. Right{ResetColor}");

            int directionChoice = GetChoice(1, 3);

            Direction direction = (Direction)(directionChoice - 1);

            if (EncounterWildPokemon())
            {
                Console.WriteLine($"{RedColor}A wild Pokemon appeared!{ResetColor}");
                Thread.Sleep(1000);

                // Wild Pokemon battle
                Pokemon wildPokemon = GenerateWildPokemon();
                Console.WriteLine($"{RedColor}A wild {wildPokemon.Name} appeared!{ResetColor}");

                Battle(playerPokemon, wildPokemon);

                if (playerPokemon.IsFainted())
                {
                    Console.WriteLine($"{RedColor}Your Pokemon fainted! Game Over.{ResetColor}");
                    break;
                }
            }

            // Move in the chosen direction
            switch (direction)
            {
                case Direction.Forward:
                    Console.WriteLine($"{GreenColor}You moved forward.{ResetColor}");
                    break;
                case Direction.Left:
                    Console.WriteLine($"{GreenColor}You moved left.{ResetColor}");
                    break;
                case Direction.Right:
                    Console.WriteLine($"{GreenColor}You moved right.{ResetColor}");
                    break;
            }

            Thread.Sleep(1000); // Pause for 1 second
        }
    }

    // Function to get a valid choice from the player
    static int GetChoice(int min, int max)
    {
        int choice;
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out choice) && choice >= min && choice <= max)
            {
                break;
            }
            Console.WriteLine($"{RedColor}Please enter a number between {min} and {max}.{ResetColor}");
        }
        return choice;
    }

    // Function to check if a wild Pokemon is encountered
    static bool EncounterWildPokemon()
    {
        Random random = new Random();
        return random.Next(1, 11) <= 3; // 30% chance of encountering a wild Pokemon
    }

    // Function to generate a wild Pokemon
    static Pokemon GenerateWildPokemon()
    {
        Random random = new Random();
        switch (random.Next(1, 4))
        {
            case 1:
                return new Pokemon("Pidgey", 3, Type.Grass, new Attack[] { new Attack("Peck", 10, Type.Grass), new Attack("Tackle", 10, Type.Grass) });
            case 2:
                return new Pokemon("Rattata", 3, Type.Fire, new Attack[] { new Attack("Scratch", 10, Type.Fire), new Attack("Quick Attack", 10, Type.Fire) });
            case 3:
                return new Pokemon("Caterpie", 3, Type.Water, new Attack[] { new Attack("Tackle", 10, Type.Water), new Attack("String Shot", 10, Type.Water) });
            default:
                return null;
        }
    }

    // Function to handle a battle between two Pokemon
    static void Battle(Pokemon playerPokemon, Pokemon opponentPokemon)
    {
        while (true)
        {
            Console.WriteLine($"{GreenColor}Your {playerPokemon.Name} HP: {playerPokemon.CurrentHP}{ResetColor}");
            Console.WriteLine($"{GreenColor}Opponent's {opponentPokemon.Name} HP: {opponentPokemon.CurrentHP}{ResetColor}");

            Console.WriteLine($"{GreenColor}Choose your action:{ResetColor}");
            Console.WriteLine($"{GreenColor}1. Attack{ResetColor}");
            Console.WriteLine($"{GreenColor}2. Heal{ResetColor}");

            int playerActionChoice = GetChoice(1, 2);

            if (playerActionChoice == 1) // Attack
            {
                Console.WriteLine($"{GreenColor}Choose your attack:{ResetColor}");
                for (int i = 0; i < playerPokemon.Attacks.Length; i++)
                {
                    Console.WriteLine($"{GreenColor}{i + 1}. {playerPokemon.Attacks[i].Name}{ResetColor}");
                }

                int playerAttackChoice = GetChoice(1, playerPokemon.Attacks.Length) - 1;
                Attack playerAttack = playerPokemon.Attacks[playerAttackChoice];

                // Opponent's attack
                Random random = new Random();
                int opponentAttackChoice = random.Next(0, opponentPokemon.Attacks.Length);
                Attack opponentAttack = opponentPokemon.Attacks[opponentAttackChoice];

                // Calculate damage considering type advantages
                int playerDamageDealt = CalculateDamage(playerAttack, opponentPokemon.PokemonType);
                int opponentDamageDealt = CalculateDamage(opponentAttack, playerPokemon.PokemonType);

                opponentPokemon.TakeDamage(playerDamageDealt);
                playerPokemon.TakeDamage(opponentDamageDealt);

                Console.WriteLine($"{GreenColor}You used {playerAttack.Name}!{ResetColor}");
                Console.WriteLine($"{GreenColor}Opponent used {opponentAttack.Name}!{ResetColor}");
            }
            else if (playerActionChoice == 2) // Heal
            {
                if (playerPokemon.CurrentHP == playerPokemon.MaxHP)
                {
                    Console.WriteLine($"{RedColor}Your Pokemon's HP is already full!{ResetColor}");
                    continue; // Continue the loop, prompting the player to choose an action again
                }

                int healAmount = 20; // Amount of HP to be healed
                playerPokemon.CurrentHP += healAmount;
                if (playerPokemon.CurrentHP > playerPokemon.MaxHP)
                    playerPokemon.CurrentHP = playerPokemon.MaxHP;

                Console.WriteLine($"{GreenColor}You healed your {playerPokemon.Name} for {healAmount} HP!{ResetColor}");
            }

            Thread.Sleep(1000); // Pause for 1 second

            if (opponentPokemon.IsFainted())
            {
                Console.WriteLine($"{GreenColor}Opponent's {opponentPokemon.Name} fainted! You win!{ResetColor}");
                playerPokemon.GainExp(100);
                Console.WriteLine($"{GreenColor}You gained 100 EXP!{ResetColor}");
                break;
            }

            if (playerPokemon.IsFainted())
            {
                Console.WriteLine($"{RedColor}Your {playerPokemon.Name} fainted! You lose!{ResetColor}");
                break;
            }
        }
    }

    // Function to calculate damage based on type advantages
    static int CalculateDamage(Attack attack, Type defendingType)
    {
        double effectiveness = 1.0;

        switch (attack.AttackType)
        {
            case Type.Grass:
                if (defendingType == Type.Water)
                    effectiveness = 0.5;
                else if (defendingType == Type.Fire)
                    effectiveness = 2.0;
                break;
            case Type.Fire:
                if (defendingType == Type.Grass)
                    effectiveness = 0.5;
                else if (defendingType == Type.Water)
                    effectiveness = 2.0;
                break;
            case Type.Water:
                if (defendingType == Type.Fire)
                    effectiveness = 0.5;
                else if (defendingType == Type.Grass)
                    effectiveness = 2.0;
                break;
        }

        return (int)(attack.Damage * effectiveness);
    }
}