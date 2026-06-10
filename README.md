# Project Slime Roller

## Members: 
1. **Lead Systems, Abilities Programmer/Artist**: Edison Chan - `Edison272`
2. **AI Programmer & Gameplay Programmer** - Tristan Chen - `TristanChenUCSC`
3. **Gameplay/Hazards Programmer** - Stanley Hung - `Stan-21`
4. **Systems Programmer & UI Developer**: Joshua Kim-Pearson - `JoshuWoshua1`
5. **Level & World Designer/Programmer** Lorenzo Uk - `lorenzouk`
6. **Shaders & Collectables Programmer/Artist**: Brody Vance - `blvance`

---

## Graphics Integration:

### Lighting:
- `Rooms` - Simple directional lights and point lights create a dark atmosphere illuminated by artificial lights
- `Drones & Cameras` - Drones and Cameras have lights to represent the different states their AI is in. Yellow for Patrolling, Red for Actively Attacking, and Blue for Stunned
- `Slime` -  Each Slime Powerup/State causes the slime to emit a different colored light. Green for Basic/Jump, Red for Chrome, Purple for Gravity, Blue for Electric Shock, and no light for Shadow

### Shaders:
- `Laser Shader` - For the lasers, We made a simple shader that scrolls a texture left to right and emits a bright HDR color to give off a glowing / shooting effect.
- `SlimeMembrane “Container” Shader` - Uses a normal map + metallic reflection in order to define the container’s shape despite being fully transparent.
- `SlimeCore “Goo” Shader` - Simulates basic liquid physics. Gives the object’s material a “fill” level like filling water into a cup, by setting transparency based on the pixel’s object position. Uses a custom script combined with rotational offsets on the x & z axis to simulate the a “liquid wobbling” effect whenever the object moves
- `ShockCore Shader` - Applies a custom weighted noisemap to the positions of the model’s pixel positions to create a “spiky” effect. Using a shifting tilemap causes the spikes to flicker like electric sparks. HDR Colors and fresnel effect allow the material to glow.

### Particles:
- `Spark Particles` - used in the EMP effect of the shock slime as well as the broken doors scene across various levels
- `Shade Particles` - used by the Shade/Shadow/Ninja slime whenever they disappear and reappear from sight

## Audio Assets

### Sound Effects:
- My Instants
- Pixabay
- Roblox
- Star Wars

### Music Used:
- Irving Heat - Stealth Music Beats
- FilFar - Bitrack 


