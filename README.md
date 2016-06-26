# PkMn.Game

PkMn.Game is a graphical implementation of generation I Pokémon battles. **It uses the PkMn.Model and PkMn.Instnace libraries whose source code can be found in the repository [PkMn](../PkMn).**

## License

Pokémon and any and all related names, assets, and designs are trademark ™ and/or copyright © 1995-present The Pokémon Company / Nintendo / Creatures Inc. / GAME FREAK Inc.

Remaining program code is copyright © 2016 Matthew Blaine and released under the terms of the MIT License. Please see: [LICENSE.txt](LICENSE.txt).

## Controls

##### Keyboard
Use the arrow keys to navigate menus, enter or space to submit, and escape to cancel or back out of a menu.

Additionally you can use the keys W and E to switch into and out of a widescreen mode where additional heads-up display information is shown. This information includes current stat stages and effective stat numbers. This is information as it was calulated in the original games, though it wouldn't show it to you.

##### Gamepad
Use the left thumbstick or directional pad to navigate menus, the A button to submit, and B to cancel or back out of a menu.

##### General notes

When the computer is sending out a new Pokémon and the game asks if you want to switch, it does not provide a yes or no option and instead sends you into the party menu either way. Backing out with escape (keyboard) or B (gamepad) or selecting the same Pokémon that is currently out is the equivalent to saying "no".

## Download

Visit [**releases**](releases) to download the latest version.

## Screenshot

![In game screenshot](raw/master/screenshot.png)


## Picking your team

By default it will generate both the player's and computer's teams randomly. In the same location as PkMn.Game.exe there can optionally be another file, parties.xml. There are a few examples included in the released zip file. 

The program will only look at parties.xml, if it exists. Rename or copy the contents of the other example parties_*.xml files to try out other configurations, or create your own.

## Important notes

### Editing the configuration files

If editing parties.xml or any of the other XML files under in the Generation-I folder note that there is very little error handling surrounding the parsing of these files. Typos will cause the program to crash at startup and likely won't provide any useful error message. The config files being formatted correctly was just an assumption I made and never revisited, since these are things that wouldn't change all that often. Pokémon battles rarely devolve into spelling bees... heh.

Also species, moves, and element types refer to each other by name rather than by any ID number. So, for example, editing an individual Pokémon's moves in parties.xml or its species definition in Generation-I\species.xml so that it knows or can learn the move *"Stranth"* instead of *"Strength"* will unfortunately crash the program outright. By using their English names as keys it made the configs easier to read as you don't have to memorize or look up species and move ID numbers.

If using a parties.xml file to configure your team, the program will try to generate data for any information left out. So if you leave the stats and/or moves blank for a specific Pokémon it will generate them as the original games would have based on its species and level.

Additionally if using a parties.xml file teams can have more than 6 Pokémon however the only limitation is the player's party menu is only big enough to show 6.

### How random teams are generated

Moves for the computer's team, when generated randomly, are filled in as a trainer or wild Pokémon would be generated in the original games. Which means they'll have the up to four moves they would have most recently learned at their current level.

Then for the player's team it does the same but I've also added an arbitrary chance for it to replace one of the Pokémon's moves with a move from the list of TMs and HMs its species can learn. It only does this when generating a move set for a team member from scratch, not if at least one move was specified for it in a parties.xml file. This was to make the game more interesting since this program only does battles and there's no opportunity to learn many of the moves otherwise. Also I noticed that in generation I for species that evolve via an evolution stone it pretty much assumes it already learned most of it's moves before evolving. So for instance a wild Raichu in the original games will only know Thunder Shock, Growl, and Thunder Wave. Which isn't very much fun.

The chance for a TM or HM to be randomly used is as follows:

* 100% if the Pokémon knows fewer than four moves (note that for something like Metapod it can't learn anything by TM or HM and will only know Harden no matter what)
* 60% if it has no further evolutions
* 30% otherwise, meaning if it still has at least one evolution ahead of it

Then there's a 60% chance it will grab a move that matches one of its elemental types if that's possible. Otherwise it picks a random TM or HM of a different type.

### Enemy AI

There is not any special logic behind the computers decisions. It picks moves entirely at random. Whenever a Pokémon faints it will always choose the next one in its party.

### Sound

There is no audio in this game, I do not have plans to add any sound effects or music at this time.

## Resources

These are the main resources I used while building all of this.

* <https://github.com/pret/pokered>
* <http://bulbapedia.bulbagarden.net/>
* <https://pokemonshowdown.com/damagecalc/>
* <http://www.smogon.com/dex/rb/pokemon/>

## Source

The source for PkMn.Game is available at <https://github.com/mblaine/PkMn.Game>. The source for the PkMn.Model and PkMn.Instance libraries is available at <https://github.com/mblaine/PkMn>.