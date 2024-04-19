## Authors

**Ford Marklew - All programming and prototyping**

## Built With
Unity 2022.3.16

## Game Notes
* The focus was game feel - things like shooting and rolling had to feel good for the player. Given the constraint to work with mobile, and in portrait mode - this was an interesting challenge.
* Weapon and projectile systems can be used together to create any weapon and ammunition type
* Weapon pickup system is easy to work with
* There are two editor tools made, both leveraged ChatGPT
    * Door Area Scanner - allows doors to be easily created and pathed through via trigger boxes. 
    * Tilemap Collider Editor - The default behaviour for Tilemaps uses mesh colliders, which is less efficient and also unnecessary for this game. The tool adds a normal box collider for each entry in the tilemap

## Notes for improvement
* Camera could be a little closer to Enter The Gungeon by following the player directly unless aim is detected. As it stands there is still a minor difference where the camera is always "chasing" the player which can be improved. ETG also seems to have some special camera logic for dodging which I did not have time to implement
* No work was done on UI (each gun should have a custom UI it loads and overrides). This could be done by adding a HUD Controller with public static event similar to the PlayerWeaponHandler.cs and having a hud prefab optionally available in Weapon.cs
* Could add more character animations to make walking around feel better.
* Could add audio for better player feedback

## Acknowledgments
* **Jacub Day - Character Art**
  * The character art used in this game was provided by Jacub Day. A colleague of mine who was kind enough to make the character and dodge roll artwork.
* **Kartoy - Tileset**
  * The tileset used in the game was created by Kartoy and is free for use (https://kartoy.itch.io/32x32sandstone-dungeon-and-character-pack). This tileset forms the visual foundation of the game’s environments. 
