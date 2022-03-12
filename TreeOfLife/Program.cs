// This goal of this project is to create a Text-Base Adventure game WITHOUT object-oriented programming.
// Why no OOP? Because my first-semester programming students are tasked with creating a text game without OOP.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TreeOfLife
{
    class Program
    {

        const ConsoleColor COLOR_NARRATE = ConsoleColor.Gray;
        const ConsoleColor COLOR_DIALOG = ConsoleColor.Green;
        const ConsoleColor COLOR_KEYWORD = ConsoleColor.Magenta;
        const ConsoleColor COLOR_INPUT = ConsoleColor.White;
        const ConsoleColor COLOR_PROMPT = ConsoleColor.DarkGray;
        const ConsoleColor COLOR_LOCATION = ConsoleColor.Yellow;

        const bool DEV = true;

        const int COLUMN_L = 3;
        const int COLUMN_R = 100;
        static int SCREEN_W = 150;
        static int SCREEN_H = 50;


        enum Location
        {
            AcaciaVillage,
            NolasHome,
            EldersMonastery,
            TheBranch,
            EdgarsNest,
            MossLake,
            ExplorersLanding,
            WeaversDen,
            TrunkWall,
            BanyanCity,
            LilliansKeep,
            Marketplace,
            RootFissures,
            ProspectorsCanyon,
            MitesCave,
            Prison
        }
        static string[] locationNames = {
            "Acacia Village",
            "Nola's Home",
            "The Elders' Monastery",
            "The Branch",
            "Edgar's Nest",
            "Moss Lake",
            "Explorers' Landing",
            "Weaver's Den",
            "The Trunk Wall",
            "Banyan City Square",
            "Lillian's Keep",
            "The Marketplace",
            "The Root Fissures",
            "Prospectors' Canyon",
            "Mite's Cave",
            "The Prison"
        };
        enum Items {
            FishingPole,
            TrunkMap,
            Wingsuit,
            Feather,
            RottenEgg,
            Aphids,
            KelpMoss,
            RustyKnife,
            SilkSack,
            AntsPants,
            FlameGel,
            CorrosiveAcid,
            PurpleLichen,
            MarketplaceContract,
            Lantern
        }

        static Random rand = new Random(); // a random number generator
        
        static Location location = Location.AcaciaVillage; // the player's location
        static List<Location> locationsDiscovered = new List<Location>();
        static List<Items> inventory = new List<Items>();
        
        static bool useTypeWriter = true;
        static bool shouldQuit = false;
        
        #region Core
        static void Main(string[] args)
        {
            ResetColors();
            
            Console.Clear();
            Console.InputEncoding = Console.OutputEncoding = Encoding.Unicode;

            if (SCREEN_W > Console.LargestWindowWidth) SCREEN_W = Console.LargestWindowWidth;

            Console.SetWindowSize(SCREEN_W, SCREEN_H);
            Console.CursorVisible = false;

            if(!DEV) CutsceneIntro();

            Console.WriteLine("\n");
            while (!shouldQuit)
            { // begin game loop
                TakeTurn();
            } // end game loop
        } // end Main()
        static void TakeTurn()
        {
            // input:
            string input = Prompt();

            // output:
            ResetColors();
            if (CommonCommands(input)) return;
            switch (location)
            {
                case Location.AcaciaVillage: AcaciaVillage(input); break;
                case Location.BanyanCity: BanyanCity(input); break;
                case Location.EdgarsNest: EdgarsNest(input); break;
                case Location.EldersMonastery: EldersMonastery(input); break;
                case Location.ExplorersLanding: ExplorersLanding(input); break;
                case Location.LilliansKeep: LilliansKeep(input); break;
                case Location.Marketplace: Marketplace(input); break;
                case Location.MitesCave: MitesCave(input); break;
                case Location.MossLake: MossLake(input); break;
                case Location.NolasHome: NolasHome(input); break;
                case Location.Prison: Prison(input); break;
                case Location.ProspectorsCanyon: ProspectorsCanyon(input); break;
                case Location.RootFissures: RootFissures(input); break;
                case Location.TheBranch: TheBranch(input); break;
                case Location.TrunkWall: TrunkWall(input); break;
                case Location.WeaversDen: WeaversDen(input); break;
            }
        }
        static string Prompt()
        {
            Console.ForegroundColor = COLOR_PROMPT;
            Console.Write("\n\n\n Nola @ ");
            PrintLocation(locationNames[(int)location]);
            Console.ForegroundColor = COLOR_PROMPT;
            Console.Write(" > ");
            Console.ForegroundColor = COLOR_INPUT;
            Console.CursorVisible = true;

            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true); // clear the input buffer
            }
            ScrollDown(3);

            string input = Console.ReadLine().ToLower();
            Console.CursorVisible = false;
            Console.WriteLine();

            useTypeWriter = true;
            return input;
        }
        static bool CommonCommands(string input)
        {
            switch (input)
            {
                case "?":
                case "help":
                case "commands":
                    PrintLocation("  == How to Play ==");
                    PrintNarrative("\n  Type in commands and press enter.");
                    PrintLocation("\n\n  == Common Commands ==");
                    PrintKeyword("\n LOOK\t"); PrintNarrative("Describes your current location.");
                    PrintKeyword("\n TALK\t"); PrintNarrative("Talk to someone in your current location.");
                    PrintKeyword("\n N\t"); PrintNarrative("Travel North");
                    PrintKeyword("\n E\t"); PrintNarrative("Travel East");
                    PrintKeyword("\n S\t"); PrintNarrative("Travel South");
                    PrintKeyword("\n W\t"); PrintNarrative("Travel West");

                    PrintKeyword("\n\n MAP\t"); PrintNarrative("View world map");
                    PrintKeyword("\n INV\t"); PrintNarrative("View inventory");
                    
                    PrintKeyword("\n\n EXIT\t"); PrintNarrative("Quit the game");

                    return true;
                case "quit":
                case "exit":
                    Clear();
                    
                    int n = MultipleChoice(
                        "Do you really want to end your journey?",
                        new string[] { "Yes, quit the game", "No, keep playing" },
                        ConsoleColor.Red);

                    shouldQuit = (n == 0);
                    
                    Clear();
                    return true;
                case "map":
                    DrawMap(SCREEN_W/2- 27, Console.CursorTop, true);
                    break;
                case "inventory":
                case "inv":
                case "backpack":
                case "pack":
                case "i":
                    DrawInventory();
                    return true;
                case "clear":
                    Console.Clear();
                    return true;
            }
            return false;
        }
        #endregion
        #region Locations
        static void AcaciaVillage(string input)
        {
            switch (input)
            {
                case "look":
                    PrintNarrative("You are in Acacia village, a cluster treehouses clinging to the sides of three, thick boughs. The villagers' call this region \"the great fork\", since these somewhat parallel limbs split apart a common source.");
                    PrintNarrative("\n\n  The village center is a round platform suspended in place with rope and rigging. It hangs down a bit from a great fork in the branch above.");
                    PrintNarrative(" The center is remarkable in both its size and its flatness. On more than one occassion the entire town has been able to stand and assemble together on this wonder of arboreal engineering.");
                    PrintNarrative(" From here, a network of of boardwalks and bridges scaffold across and around the branches, connecting the homes and shops that cling to the branches of the great fork.");
                    PrintNarrative(" The boardwalks and the village center itself are constructed of beams of (relatively tiny) timber.");
                    PrintNarrative("\n\n  The village center is mostly quiet today, apart from the leaves rustling and boards creaking.");
                    PrintNarrative("\n  A few people meander about, but today there are no merchants peddling food or crafts.");
                    PrintNarrative("\n  Near the far perimeter, "); PrintKeyword("JOEY"); PrintNarrative(" lays in a hammock.");

                    PrintNarrative("\n ");

                    break;
                case "joe":
                case "joey":
                case "talk to joe":
                case "talk to joey":
                case "talk joey":
                case "talk joe":
                    PrintDialog("Hi Nola! I'm sorry to see you go, but I know you'll find us a great new place to live!");
                    break;
                case "n":
                case "north":
                    ChangeLocation(Location.EldersMonastery, "");
                    break;
                case "e":
                case "east":
                    ChangeLocation(Location.NolasHome, "You set off across the village. After crossing a few hanging bridges, you come to your home. For a moment you stand and admire the little one-room house before stepping inside.");
                    break;
                case "s":
                case "south":
                    ChangeLocation(Location.TheBranch, "");
                    break;
                case "w":
                case "west":

                    break;
            }
        }
        static void BanyanCity(string input)
        {
            switch (input)
            {
                case "look":
                    
                    break;
                case "n":
                case "north":
                    ChangeLocation(Location.TrunkWall, "");
                    break;
                case "w":
                case "west":
                    ChangeLocation(Location.LilliansKeep, "");
                    break;
                case "e":
                case "east":
                    ChangeLocation(Location.Marketplace, "");
                    break;
                case "s":
                case "south":
                    ChangeLocation(Location.RootFissures, "");
                    break;
            }
        }
        static void EdgarsNest(string input)
        {
            switch (input)
            {
                case "look":

                    if (SeeIsFirstTime(Location.EdgarsNest)) {
                        PrintNarrative("A giant bird looms over you!");
                        PrintNarrative("He introduces himself as Edgar.");
                    } else {
                        PrintNarrative("You're standing in Edgar's nest.");
                    }
                    break;
                case "w":
                case "west":
                    ChangeLocation(Location.TheBranch, "");
                    break;
            }
        }
        static void EldersMonastery(string input)
        {
            switch (input)
            {
                case "look":
                    break;
                case "s":
                case "south":
                    ChangeLocation(Location.AcaciaVillage, "");
                    break;
            }
        }
        static void ExplorersLanding(string input)
        {
            switch (input)
            {
                case "look":
                    break;
                case "e":
                case "east":
                    ChangeLocation(Location.MossLake, "");
                    break;
            }
        }
        static void LilliansKeep(string input)
        {
            switch (input)
            {
                case "look":
                    break;
                case "e":
                case "east":
                    ChangeLocation(Location.BanyanCity, "");
                    break;
                case "s":
                case "south":
                    ChangeLocation(Location.Prison, "");
                    break;
            }
        }
        static void Marketplace(string input)
        {
            switch (input)
            {
                case "look":
                    break;
                case "w":
                case "west":
                    ChangeLocation(Location.BanyanCity, "");
                    break;
            }
        }
        static void MitesCave(string input)
        {
            switch (input)
            {
                case "look":
                    break;
                case "s":
                case "south":
                    ChangeLocation(Location.ProspectorsCanyon, "");
                    break;
            }
        }
        static void MossLake(string input)
        {
            switch (input)
            {
                case "look":
                    
                    
                    PrintNarrative("You stand on the Eastern edge of a large green pond which the villagers call Moss Lake. Here, the cracked bark of the branch gently slopes down into the water, but along the Northern banks, large clumps of moss jut out from the water like large, thick bushes.");
                    PrintNarrative("\n\n The far edge of the pond meets the vertical trunk of the tree. Where the branch joins the great trunk, the limbs form a wide crevasse. Hidden between great cracks in the bark, a few rivulets trickle down the trunk and feed a shallow pool about 30 feet above Moss Lake. Out from this pool, a waterfall spills into the crevasse, creating the Lake.");
                    PrintNarrative("\n\n Throughout your youth, this had been a particularly relaxing place to pass the time. You recall several trips to Moss Lake and the hours spent swimming and fishing.");
                    PrintNarrative("\n\n Now, the Lake looks overgrown with moss and algae, and you struggle to see any fish in the green waters.");

                    if (HasItem(Items.FishingPole))
                    {
                        PrintNarrative("\n\n You could try to use your fishing pole, but you doubt that you would have much luck. Maybe if you could find a way to lure the fishes out from the moss and algae...");
                    }

                    PrintNarrative("\n\n To the "); PrintKeyword("EAST");PrintNarrative(" there is the branch trail which leads back to the village.");
                    PrintNarrative("\n To the "); PrintKeyword("WEST");PrintNarrative(", dilapidated stairs appear to ascend the trunk, winding off into the distance, around the curve of the tree.");
                    PrintNarrative("\n To the "); PrintKeyword("SOUTH");PrintNarrative(", a worn trail plunges steeply down the trunk of the tree into the clouds below.");

                    break;
                case "s":
                case "south":
                    ChangeLocation(Location.TrunkWall, "");
                    break;
                case "w":
                case "west":
                    ChangeLocation(Location.ExplorersLanding, "");
                    break;
                case "e":
                case "east":
                    ChangeLocation(Location.TheBranch, "");
                    break;
            }
        }
        static void NolasHome(string input)
        {
            switch (input)
            {
                case "look":
                    PrintNarrative("Nola's shack gently sways back and forth. ");
                    PrintNarrative("");
                    break;
                case "w":
                case "west":
                    ChangeLocation(Location.AcaciaVillage, "You secure the door, and walk towards the center of town.");
                    break;
            }
        }
        static void Prison(string input)
        {
            switch (input)
            {
                case "n":
                case "north":
                    ChangeLocation(Location.LilliansKeep, "");
                    break;
                case "look":
                    break;
            }
        }
        static void ProspectorsCanyon(string input)
        {
            switch (input)
            {
                case "look":
                    break;
                case "n":
                case "north":
                    ChangeLocation(Location.MitesCave, "");
                    break;
                case "w":
                case "west":
                    ChangeLocation(Location.RootFissures, "");
                    break;
            }
        }
        static void RootFissures(string input)
        {
            switch (input)
            {
                case "look":
                    break;
                case "n":
                case "north":
                    ChangeLocation(Location.BanyanCity, "");
                    break;
                case "e":
                case "east":
                    ChangeLocation(Location.ProspectorsCanyon, "");
                    break;
            }
        }
        static void TheBranch(string input)
        {
            switch (input)
            {
                case "look":
                    
                    break;
                case "w":
                case "west":
                    ChangeLocation(Location.MossLake, "");
                    break;
                case "n":
                case "north":
                    ChangeLocation(Location.AcaciaVillage, "");
                    break;
                case "e":
                case "east":
                    ChangeLocation(Location.EdgarsNest, "");
                    break;
            }
        }
        static void TrunkWall(string input)
        {
            switch (input)
            {
                case "look":
                    
                    break;
                case "n":
                case "north":
                    ChangeLocation(Location.MossLake, "");
                    break;
                case "s":
                case "south":
                    ChangeLocation(Location.BanyanCity, "");
                    break;
            }
        }
        static void WeaversDen(string input)
        {
            switch (input)
            {
                case "look":
                    break;
            }
        }
        #endregion
        #region Drawing
        static void DrawBox(int x, int y, int w, int h) {
            //   _____________
            //   )            ) 
            //  (            (
            //   )____________)

            if (h % 2 == 1) h++;

            StringBuilder sb = new StringBuilder();
            for(int yy = 0; yy < h; yy++) {
                for(int xx = 0; xx < w; xx++) {
                    bool isFirst = (xx == 0);
                    bool isLast = (xx == w - 1);
                    if(yy == 0) {
                        if (isFirst) sb.Append(" _");
                        else if (isLast) sb.Append(" ");
                        else sb.Append("_");
                    }
                    else if(yy == h - 1) {
                        if (isFirst) sb.Append(" )");
                        else if (isLast) sb.Append(")");
                        else sb.Append("_");
                    } else {
                        if(yy % 2 == 1) {
                            if (isFirst) sb.Append(" )");
                            else if (isLast) sb.Append(")");
                            else sb.Append(" ");

                        } else {
                            if (isFirst) sb.Append("(");
                            else if (isLast) sb.Append("(");
                            else sb.Append(" ");
                        }
                    }
                }
                Console.SetCursorPosition(x, y+yy);
                Console.Write(sb.ToString());
                sb.Clear();
            }

        }
        static void DrawMap(int x, int y, bool showPosition = true)
        {
            int preX = Console.CursorLeft;
            int preY = Console.CursorTop;

            if(showPosition) DrawCompass();

            //if (showPosition) Clear();
            Console.SetCursorPosition(x, y);

            string[] tree = {
                @"             ~#####",
                @"          ##############",
                @"        ~#############~",
                @"          ##~    \ \                 ##  ######~",
                @"            #####~ ###.        _--##################",
                @"         ##############-_    .' _.-#############~###~",
                @"       ################. \._/  /       ##############",
                @"        ####~~   ###    \_    / ##~         ~####",
                @"                          \  |####",
                @"                          |  #/#     ~#####~~",
                @"         ######          /  /  _.--#############~",
                @"     ~############/--.  |  |  /  .-##############~",
                @"    ###########^----\ \_-  `-' .--.####~~#######",
                @"   ###~~###~          --.      /    ~######~",
                @"    ~####             /      /",
                @"                     /       |",
                @"                    |     ,  \",
                @"                  ,'  _.._   |\`.",
                @"               _-` _,`   /  /`-.\`,",
                @"              /_--'   _,@- |    `. ',",
                @"             .`    _ /  ,-. \      \ \",
                @"              `-_.  \_ /   `.\.     |@",
                @"                /\_   `-..          ./",
                @"             _-'_/  `-,_\        _.'",
                @"            /_-'  _.-/__/`-.__.-'\_",
                @"                 /       `\    '-. \_",
                @"                 '-.,___  /       '-.\",
                @"                    '. \`'",
                @"                      `v"
            };
            foreach (string line in tree)
            {
                foreach (char c in line)
                {
                    ResetColors();
                    if (c == '#') Console.ForegroundColor = ConsoleColor.DarkGreen;

                    if (c == '&') Console.ForegroundColor = ConsoleColor.Black;
                    if (c == '$') Console.ForegroundColor = ConsoleColor.Black;
                    if (c == '*') Console.ForegroundColor = ConsoleColor.Black;

                    if (c == ' ')
                    {
                        Console.CursorLeft++;
                    }
                    else
                    {
                        Console.Write(c);
                    }
                }
                Console.CursorTop++;
                Console.CursorLeft = x;
            }
            if (!showPosition) return;

            switch (location)
            {
                case Location.AcaciaVillage:        Console.SetCursorPosition(40 + x, 10 + y); break;
                case Location.BanyanCity:           Console.SetCursorPosition(25 + x, 17 + y); break;
                case Location.EdgarsNest:           Console.SetCursorPosition(35 + x, 12 + y); break;
                case Location.EldersMonastery:      Console.SetCursorPosition(39 + x, 09 + y); break;
                case Location.ExplorersLanding:     Console.SetCursorPosition(30 + x, 08 + y); break;
                case Location.LilliansKeep:         Console.SetCursorPosition(21 + x, 17 + y); break;
                case Location.Marketplace:          Console.SetCursorPosition(26 + x, 17 + y); break;
                case Location.MitesCave:            Console.SetCursorPosition(30 + x, 20 + y); break;
                case Location.MossLake:             Console.SetCursorPosition(27 + x, 12 + y); break;
                case Location.NolasHome:            Console.SetCursorPosition(42 + x, 10 + y); break;
                case Location.Prison:               Console.SetCursorPosition(18 + x, 18 + y); break;
                case Location.ProspectorsCanyon:    Console.SetCursorPosition(28 + x, 21 + y); break;
                case Location.RootFissures:         Console.SetCursorPosition(20 + x, 20 + y); break;
                case Location.TheBranch:            Console.SetCursorPosition(31 + x, 11 + y); break;
                case Location.TrunkWall:            Console.SetCursorPosition(25 + x, 14 + y); break;
                case Location.WeaversDen:           Console.SetCursorPosition(27 + x, 10 + y); break;
            }
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("(X)");
            Console.SetCursorPosition(0, y + tree.Length);

            ResetColors();
        }
        static void DrawTitle(int x, int y)
        {
            //TODO: refactor this function to wrap using console.cursorleft
            ResetColors();

            DrawTitle1(x, y);
            Thread.Sleep(500);
            DrawTitle2(x, y, ConsoleColor.DarkGray);
            Thread.Sleep(200);
            DrawTitle2(x, y, ConsoleColor.Gray);
            Thread.Sleep(200);
            DrawTitle2(x, y, ConsoleColor.White);
            Thread.Sleep(200);
            DrawTitle3(x, y);
        }
        static void DrawTitle1(int x, int y)
        {
            int ty = y;

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            string[] pieces1 = {
                @"~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~.",
                @"  ~                                                                      ~   |~~~.",
                @"       ~                ~                                       ~            |    |~~~~~~~.------<>-------<>",
                @"                                 ~                      ~           ~        | ~  |    ~ /",
                @"           ~                                                              ~  |    |      \---<>--<>",
                @"                   ~                              ~                          |    | ~    /",
                @"         ~                               ~                        ~          |  ~ |      \-----<>----<>",
                @"             ~                                                       ~     ~ |    |   ~  /",
                @"      ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~' ~   |      \---<>--<>",
                @"                                                                     `~~~~~~~~~~~'   ~   /",
                @"                                                                         `~~~~~~~~~~~~~~~'-------<>-------<>"
            };

            for (int i = 0; ; i++)
            {
                ty = y;
                bool isDone = true;
                foreach (string piece in pieces1)
                {
                    char c = i < piece.Length ? piece[i] : ' ';

                    if (c != ' ')
                    {
                        isDone = false;
                        Console.SetCursorPosition(x + i, ty);
                        Console.Write(c);
                    }
                    ty++;
                }
                if (isDone) break;
                Thread.Sleep(10);
            }
        }
        static void DrawTitle2(int x, int y, ConsoleColor color)
        {

            int ty = y;
            string[] pieces2 = {
                @" _______                     __   _      _  __",
                @"|__   __|                   / _| | |    (_)/ _|",
                @"   | |_ __ ___  ___    ___ | |_  | |     _| |_ ___",
                @"   | | '__/ _ \/ _ \  / _ \|  _| | |    | |  _/ _ \",
                @"   | | | |  __/  __/ | (_) | |   | |____| | ||  __/",
                @"   |_|_|  \___|\___|  \___/|_|   |______|_|_| \___|",
            };
            Console.ForegroundColor = color;
            ty = y + 1;
            foreach (string piece in pieces2)
            {
                Console.SetCursorPosition(x + 12, ty);
                foreach (char c in piece)
                {
                    if (c == ' ')
                        Console.CursorLeft++;
                    else
                        Console.Write(c);
                }
                ty++;
            }
        }
        static void DrawTitle3(int x, int y)
        {

            string[] lines = {
                "'.__|   |                           |   |__.'",
                "    '.__|  A game by Nick Pattison  |__.'",
                "        '._________________________.'"
            };

            MultiLineDraw(x + 15, y + 9, lines, ConsoleColor.Red);
            
        }
        static void MultiLineDraw(int x, int y, string[] lines, ConsoleColor color)
        {
            MultiLineDraw(x, y, lines, new Dictionary<char, ConsoleColor> { { '\0', color } });
        }
        static void MultiLineDraw(int x, int y, string[] lines, Dictionary<char, ConsoleColor> colorMap = null)
        {
            foreach (string line in lines) {
                Console.SetCursorPosition(x, y);
                foreach (char c in line)
                {
                    ColorMapLookup(c, colorMap);
                    Console.Write(c);
                }
                y++;
            }
        }
        static void ColorMapLookup(char c, Dictionary<char, ConsoleColor> map)
        {
            if (map == null) return;
            if (map.ContainsKey(c)) Console.ForegroundColor = map[c];
            else if(c != '\0') ColorMapLookup('\0', map);
        }
        static void DrawInventory()
        {
            int x = Console.CursorLeft;
            int y = Console.CursorTop;

            //Console.Clear();
            string[] lines = {
                @"  .-.---                                 -----.",
                @" /   \                                         \",
                @" \   /                                         /",
                @"  '-'---                                 -----'"
            };
            MultiLineDraw(x, y, lines, ConsoleColor.Blue);

            lines = new string[]{
                @".~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~.",
                @" \                               \",
                @" /                               /",
                @"'~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~'",
                @" |                             |",
                @" |     .. ~ Inventory ~ ..     |",
                @" |   _______________________   |",
                @" |                             |"
            };
            MultiLineDraw(x + 8, y, lines, ConsoleColor.DarkYellow);
            y += lines.Length;

            List<string> items = new List<string>();
            if(HasItem(Items.FishingPole)         ) items.Add("|     Fishing Pole            |");
            if(HasItem(Items.TrunkMap)            ) items.Add("|     Trunk Map               |");
            if(HasItem(Items.Wingsuit)            ) items.Add("|     Relic Wingsuit          |");
            if(HasItem(Items.Feather)             ) items.Add("|     Feather                 |");
            if(HasItem(Items.RottenEgg)           ) items.Add("|     Rotten Egg              |");
            if(HasItem(Items.Aphids)              ) items.Add("|     Aphid-Covered Leaf      |");
            if(HasItem(Items.KelpMoss)            ) items.Add("|     Kelp Moss               |");
            if(HasItem(Items.RustyKnife)          ) items.Add("|     Rusty Knife             |");
            if(HasItem(Items.SilkSack)            ) items.Add("|     Silk Sack               |");
            if(HasItem(Items.AntsPants)           ) items.Add("|     Ants Pants              |");
            if(HasItem(Items.FlameGel)            ) items.Add("|     Flame Gel               |");
            if(HasItem(Items.CorrosiveAcid)       ) items.Add("|     Corrosive Acid          |");
            if(HasItem(Items.PurpleLichen)        ) items.Add("|     Purple Lichen           |");
            if(HasItem(Items.MarketplaceContract) ) items.Add("|     Marketplace Contract    |");
            if(HasItem(Items.Lantern)             ) items.Add("|     Lantern                 |");

            Dictionary<char, ConsoleColor> map = new Dictionary<char, ConsoleColor>{
                {'\0', ConsoleColor.White },
                {'|', ConsoleColor.DarkYellow },
            };

            MultiLineDraw(x + 9, y, items.ToArray(), map);
            
            y += items.Count;

            lines = new string[]{
                @"|                             |",
                @"|                             |",
                @"|     __   _       _   __     |",
                @" \   /^^\_/^\     /^\_/^^\   /",
                @"  \_/    |   \   /   |    \_/",
                @"   |     |    \_/    |     |",
                @"   O     *     |     *     O",
                @"   |           8           |",
                @"   *           |           *",
                @"               |",
                @"            o==0==o"
            };
            MultiLineDraw(x + 9, y, lines, ConsoleColor.DarkYellow);
            Console.WriteLine();
        }
        static void CutsceneIntro()
        {
            Thread.Sleep(500);
            DrawMap(0, 2, false);
            Thread.Sleep(500);
            DrawTitle(30, 17);

            Thread.Sleep(1000);
            Console.ForegroundColor = ConsoleColor.White;
            Console.CursorTop = Console.WindowHeight - 2;
            Console.CursorLeft = SCREEN_W/2 - 11;
            Console.WriteLine("Press any key to begin");
            Console.ReadKey(false);
            Clear();
        }
        static void DrawCompass() {
            switch (location) {
                case Location.AcaciaVillage:    DrawCompass("Monastery", "Home", "Branch Path", ""); break;
                case Location.NolasHome:        DrawCompass("", "", "", "Village"); break;
                case Location.EldersMonastery:  DrawCompass("", "", "Acacia Village", ""); break;
                case Location.TheBranch:        DrawCompass("Acacia Village", "Edgar's Nest", "", "Moss Lake"); break;
                case Location.EdgarsNest:       DrawCompass("", "", "", "The Branch"); break;
                case Location.MossLake:         DrawCompass("", "The Branch Trail", "The Trunk Wall", "Dilapidated Stairs"); break;
                case Location.ExplorersLanding: DrawCompass("", "Moss Lake", "", ""); break;
                case Location.WeaversDen:       DrawCompass("", "", "", ""); break;
                case Location.TrunkWall:        DrawCompass("Moss Lake", "", "Banyan City", ""); break;
                case Location.BanyanCity:       DrawCompass("Trunk Wall", "Marketplace", "Root Fissures", "Lillian's Keep");break;
                case Location.LilliansKeep:     DrawCompass("", "Banyan City", "Prison", ""); break;
                case Location.Marketplace:      DrawCompass("", "", "", "Banyan City"); break;
                case Location.RootFissures:     DrawCompass("Banyan City", "Prospectors' Canyon", "", ""); break;
                case Location.ProspectorsCanyon:DrawCompass("Mite's Cave", "", "", "Root Fissures"); break;
                case Location.MitesCave:        DrawCompass("", "", "Prospector's Canyon", ""); break;
                case Location.Prison:           DrawCompass("Lillian's Keep", "", "", ""); break;
            }
        }
        static void DrawCompass(string north, string east, string south, string west)
        {
            //Console.Clear();

            int preX = Console.CursorLeft;
            int preY = Console.CursorTop;

            int x = COLUMN_R + 25;
            int y = preY + 2;
            if (north.Length > 0)
            {
                Console.SetCursorPosition(x - north.Length / 2, y - 2);
                PrintLocation(north);
            }
            Console.SetCursorPosition(x, y - 1);
            PrintColor("N", (north.Length > 0) ? ConsoleColor.Magenta : ConsoleColor.DarkGray);
            if (west.Length > 0)
            {
                Console.SetCursorPosition(x - west.Length - 3, y);
                PrintLocation(west);
            }
            Console.SetCursorPosition(x - 2, y);
            PrintColor("W ", (west.Length > 0) ? ConsoleColor.Magenta : ConsoleColor.DarkGray);
            Console.SetCursorPosition(x, y);
            PrintColor("☼", ConsoleColor.Yellow);
            Console.SetCursorPosition(x + 2, y);
            PrintColor("E ", (east.Length > 0) ? ConsoleColor.Magenta : ConsoleColor.DarkGray);
            if (east.Length > 0)
            {
                    PrintLocation(east);
            }
            Console.SetCursorPosition(x, y + 1);
            PrintColor("S", (south.Length > 0) ? ConsoleColor.Magenta : ConsoleColor.DarkGray);
            if (south.Length > 0)
            {
                Console.SetCursorPosition(x - south.Length / 2, y + 2);
                PrintLocation(south);
            }
            Console.SetCursorPosition(preX, preY);
        }
        #endregion
        #region Utilities

        static int MultipleChoice(string prompt, string[] choices, ConsoleColor color = ConsoleColor.Blue, ConsoleColor colorText = ConsoleColor.White) {


            int x = Console.CursorLeft = COLUMN_L;
            int y = Console.CursorTop + 1;

            int max = prompt.Length;
            foreach (string choice in choices) if (choice.Length > max) max = choice.Length;

            DrawBox(x, y, max + 12, choices.Length * 2 + 5);

            x += 5; 
            y += 2;

            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine(prompt);
            y += 2;

            int answer = 0;
            while (true) {
                Console.SetCursorPosition(x, y);
                if (answer < 0) answer = 0;
                if (answer >= choices.Length) answer = choices.Length - 1;
                for (int i = 0; i < choices.Length; i++) {
                    string choice = choices[i];
                    Console.SetCursorPosition(x, y + i * 2);
                    Console.BackgroundColor = (i == answer) ? color : ConsoleColor.Black;
                    Console.ForegroundColor = (i == answer) ? colorText : ConsoleColor.DarkGray;
                    Console.WriteLine($"  {choice}  ");
                }
                ConsoleKeyInfo cki = Console.ReadKey(true);
                switch (cki.Key) {
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.RightArrow:
                        answer++;
                        break;
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.UpArrow:
                        answer--;
                        break;
                    case ConsoleKey.Enter:
                    case ConsoleKey.Spacebar:
                        return answer;
                }
            }
        }
        
        static bool HasItem(Items item) {
            return inventory.Contains(item);
        }
        static void Pickup(Items item) {
             if(!inventory.Contains(item)) inventory.Add(item);
        }
        static void Drop(Items item) {
            inventory.Remove(item);
        }
        static bool SeeIsFirstTime(Location location) {
            if (locationsDiscovered.Contains(location)) return false;
            locationsDiscovered.Add(location);
            return true;
        }
        static void Clear()
        {
            ResetColors();
            Console.Clear();
        }
        static void ResetColors()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.BackgroundColor = ConsoleColor.Black;
        }
        static void PrintColor(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
        }
        static void NewParagraph()
        {
            Console.WriteLine("\n\n");
        }
        static void PrintNarrative(string text)
        {
            Console.ForegroundColor = COLOR_NARRATE;
            WrapTextAt(text, COLUMN_R);
        }
        static void PrintDialog(string text)
        {
            Console.ForegroundColor = COLOR_DIALOG;
            WrapTextAt(text, COLUMN_R);
        }
        static void PrintKeyword(string text)
        {
            Console.ForegroundColor = COLOR_KEYWORD;
            WrapTextAt(text, COLUMN_R);
        }
        static void PrintLocationDivider()
        {
            string text = locationNames[(int)location];
            int amt = (SCREEN_W - text.Length - 3) / 2;

            SwizzleHR(amt, '*');
            PrintLocation(" " + text + " ");
            SwizzleHR(amt, '*');
            Console.WriteLine("\n\n");
        }
        static void ChangeLocation(Location newLocation, string message)
        {
            location = newLocation;
            PrintLocationDivider();
            PrintNarrative(message);
        }
        static void SwizzleHR(int amt, char c)
        {
            bool color1 = true;
            for (int i = 0; i < amt; i++)
            {
                Console.ForegroundColor = (color1) ? ConsoleColor.DarkMagenta : ConsoleColor.Red;
                Console.Write('=');
                color1 = !color1;
            }
        }
        static void PrintLocation(string text)
        {
            Console.ForegroundColor = COLOR_LOCATION;
            Console.Write(text);
        }
        static void WrapTextAt(string text, int x)
        {
            string[] words = text.Split(' ');
            for(int i = 0; i < words.Length; i++)
            {
                string word = words[i];
                if (Console.CursorLeft != COLUMN_L && i != 0) Console.CursorLeft++;
                if (Console.CursorLeft + word.Length > x)
                {
                    Console.CursorLeft = COLUMN_L;
                    Console.CursorTop++;
                }
                else if (Console.CursorLeft < COLUMN_L) Console.CursorLeft = COLUMN_L;
                
                TypeWriter(word);
                
            }
        }
        static void TypeWriter(string text)
        {

            foreach(char c in text)
            {
                Console.Write(c);
                int sleepAmount = rand.Next(0, 10);
                while (Console.KeyAvailable)
                {
                    if(Console.ReadKey(true).Key == ConsoleKey.Enter)
                    {
                        useTypeWriter = false;
                    }
                }
                if(useTypeWriter) System.Threading.Thread.Sleep(sleepAmount);
            }
            
        }
        static void ScrollDown(int amt)
        {
            int preX = Console.CursorLeft;
            for (int i = 0; i < amt; i++) Console.Write("\n");
            Console.CursorLeft = preX;
            Console.CursorTop -= amt;
        }
        #endregion
    }
}
