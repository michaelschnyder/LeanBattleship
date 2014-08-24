LeanBattleship
==============

LeanBattleship is a simple REST-based C# server for playing Battleship with own clients. The aim is to develop clients in teams and let them play together

### Current State ###
The solution is stoll under development. You can visit [http://leanbattleship.cloudapp.net](http://leanbattleship.cloudapp.net) for fourther information.

[![Build status](https://ci.appveyor.com/api/projects/status/1rq1e6ya21phbhm5)](https://ci.appveyor.com/project/emtwo/leanbattleship)

### Game Play ###
This is the general way how tournaments are played

1. A client has to register itself to a tournament which is managed by the server. 2.
2. The server creates matches for a group of 2 players who are registered in a tournament and not playing a match.
3. Each of those players get a status api to get the matches the should play
4. A player has to initial setup  the gamefleet wihin a match
5. And has to fire whenever it's his turn.

## REST-API ##
The REST-Api supports the following actions which a client should implement

The API is available via /apî/ the current version can be identified via the url

	/ap/version

### All Tournaments ###
The list of tournaments is the entry point for every client

    GET /api/tournament

Sample response for that

	[
		{
			Id: 4,
			Name: "TournamentOne"
		},
		{
			Id: 5,
			Name: "Frida"
		}
	]

### Join / leave tournament
A player can join (multiple possible, nothing happens) the tournament by accessing the following url

	GET /api/tournament/[tournamentId]/join?playername="yourname"

When everything is ok, you will get a 200 (OK)

### Get the list matches ###
Use the following URL to get a list of matches where you should make a decission

	GET /api/tournament/[tournamentId]/mymatches
	
Do not forget to add your player name in the headers of your api call
	
	LeanBattleship-Playername : yourname

So that you get a response like

	TODO


### Setup your gamefleet ###
You should use the following url for that

	PUT /api/match/[matchId]/setup
	LeanBattleship-Playername : yourname

	[
		["A1", "A2", "A3", "A3"]
		["J8", "J9", "J10"]
	]

This can be done only once.

### Fire ###
To fire to the enemies gamefleet use the following url

	POST /api/match/[matchId]/fire/[position]
	LeanBattleship-Playername : yourname

There are 3 different response codes possible

* NotFound: There was no ship (or no ship anymore)
* Redirect: Ship hit but needs additional fire to bring it down
* OK: Ship is down 

