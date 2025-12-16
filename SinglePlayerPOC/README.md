**Challange:**

You are asked to implement a Hi-Lo game The principle of the game is as follows:
1.	The system chooses a number between [Min; Max] (the mystery number)
2.	The player proposes a number between [Min; Max]
3.	If the player's proposal is not the mystery number, the system tells the player whether: a. HI: the mystery number is > the player's guess b. LO: the mystery number is < the player's guess And the player plays again.
4.	The goal of the game is to discover the mystery number in a minimum of iterations. Finally, you are asked to add a multiplayer concept to the game. 


**Initial Proof of Concept (POC): Single-Player Guessing Game**
**Objective**
Create a console application that contains the integrated logic for both the single-player game server and the client (player).

**Requirements**

1. Difficulty Selection and Range Setting
- **Player Input:** The player must be able to select a difficulty level: Easy, Normal, Hard, or Expert.
- **Range Definition:** The selected difficulty will determine the **range size** for the mystery number:
    - **Easy**: Range size = 10
    - **Normal**: Range size = 50
    - **Hard**: Range size = 100
    - **Expert**: Range size = 1000
**Server **Setup:****
- The **server** will randomly set the **minimum** boundary (**min**).
- The **maximum** boundary (max) will be calculated as: $\text{max} = \text{min} + \text{Range Size} + 1$.
       **Example** (Easy): If $\text{min} = 25$, then $\text{max} = 25 + 10 + 1 = 36$. The range is $[\text{min}, \text{max}] = [25, 36]$.
- The **mystery number** will be randomly set within the defined range $[\text{min}, \text{max}]$.
- The server must then communicate the range $[\text{min}, \text{max}]$ to the player.

**2. Game Flow and Guessing**

- Start Game: The player must be able to explicitly start the game.
- Display Range: The player must be able to view the current range $[\text{min}, \text{max}]$.
- **Guessing**:
    - The player can make a guess (number input) without restrictions on the value (it can be outside the range).
    - The server will respond to the guess with the following feedback:
       - If the guess is lower than the mystery number: Server response is **"LO"** (Too Low).
       - If the guess is higher than the mystery number: Server response is **"HI"** (Too High).
       - If the guess is correct: Server response is **"Congrats! You win!"**
- The player must be able to **continue proposing guesses** until the mystery number is correctly guessed.

**3. Game Tracking and Exit**

- **Guess Counter:** The server will count **every guess** the player makes and communicate the current total count back to the player.
- **Quit Game Session:** The player must be able to quit the current game session at any time.
- **Quit Application:** The player must be able to exit the entire console application.
