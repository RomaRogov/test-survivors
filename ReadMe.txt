Hello!

I'm glad to finally present you my finished test assignment.
I didn't used Zenject for a while, so I'm not sure I've used the best practices, but I've dug in and had fun with this framework at least.
I made this in a few evenings, and in a sum it's about 2 working days. I've spent one evening to think out on how to combine everything, and then things starts to play out.
I've started with grouping everything to a logic groups, separate views from the game logic, and manipulate scene references.
Then I wrote player movement logic, enemy spawner, bullets spawner with a pooling, configs for all these things, and got something playing on the screen :)

To manipulate design values, I've created Game Settings as ScriptableObject in Resources forlder.
Here you can define:
 - enemy types with spawn rate, damage, speed, health etc
 - bullet properties (just one type for now)
 - player movement settings: speed, sensitivity of joystick
 - common logic balance: player health and experience multiplier per every level.

 I am sorry that I've started it so late - the season before christmas and thanksgiving is totally wild on my main job and I just didn't had enough time. Hope this is not a big deal!