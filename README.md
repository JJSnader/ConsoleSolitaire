# Console Solitaire

This is a simple .NET console version of solitaire, with the same rules as the old Windows XP version of solitaire.

## Running the app
This was built in Visual Studio, so using that is probably the easiest way to do it.
Honestly, I don't even know how else you would build it. I'm sure there's some other way to build it but have not researched. Just use Visual Studio and life will be easy.
For best display results, the console window should be 100 characters wide and 45 lines tall. If the game looks really weird from a bad window size, you can adjust the window size 
and initialize a new game to change it. If you find that you have lots of problems, all I can say is: "It works on my machine."

## App Commands   
### Card Designations
Some commands require a card or cards to be specified. Cards can be designated with the card number followed by the first letter of the card suite.
Aces, Jacks, Queens, and Kings have A, J, Q, and K as their numbers respectively rather than an actual number.
For example, the code of the Ace of Hearts is "AH", the code of the 2 of Clubs is "2C", etc. Card designations are case-insensitive.   
### Command List
Commands are listed at the bottom of the playing area, but for completeness, here they are. (Not necessarily all of them.)
Most commands have a shortcut, which is shown in parentheses next to it. Commands are not case-sensitive.   
- Flip (f): Show the next card
- Move (m): Requires a source card and destination card. For example, to move the 10 of Spades to the Jack of Hearts, the command is "M 10S JH".
- Pack (p): Moves the specified card to the appropriate pile at the top, if the specified card is the correct one. If it is not, an error will be displayed.
- Autopack (a): Automatically pack all cards that can be packed. When all cards are out of the deck and in ordered piles, this will finish the game.
- New (n): Initializes an entirely new game.
- Help (h): Displays this list of commands.
- Exit (e): Exit the game.
- Quit (q): Exit the game.
- BackColor (bc): Allows you to specify a new background color. Valid options are blue, green (default), red, gray, yellow, and black.
- Cardback: Allows you to choose a different card back to display on the deck.
- Mode: Allows you to change how many cards are flipped off the deck at once. Options are Single, Double, and Triple.
In addition, there may be some easter eggs that are only documented in the code.
#### Minor note
If you rummage around in the code you'll see some stuff for an autosolver; it's not complete and just blows up a text file at the moment.
Will it ever get finished? Who knows, maybe when I feel like playing solitaire again.
