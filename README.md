# SmallTK
OpenTK-based 2D game engine for Small Basic. The objective is to non-intrusively substitute the default GraphicsWindow for the creation of games, which is ill suited for realtime graphics.

Uses the awesome [ObjectTK][1] library!

##Features
 * Managed, yet fast game loop
 * Basic mouse and keyboard input
 * Simple but efficient sprite system

###Roadmap
I intend to polish the current features and add new ones, but always aiming for simplicity of use.
 * Add better input handling (events)
 * Expand sprite API (toggle state without deleting, reading properties, ...)
   * This includes adding functions to create sprites from basic geometric shapes, like rectangles or circles
 * Allow on-demand graphics mode, for non-realtime games* (e.g. quizzes, puzzle escape)
 * Add non-blocking sound playing, using OpenAL.

##Example

    'Binding the events to the custom subs
    GameEngine.OnLoad = Load 'Called just after the engine starts
    GameEngine.OnUpdate = Update 'Called before every frame, this is where the main game happens
    GameEngine.OnStop = Stop 'Called just after the engine stops

    Sub Load 'Initialize things
      For i = 1 To 100
        'Sprites.Create creates a sprite from the given image, and returns the identifier
        'that shall be used with other Sprite functions
        others[i] = Sprites.Create(Sprites.ContentPath + "/Textures/Bubble.png")
      EndFor
      player = Sprites.Create(Sprites.ContentPath + "/Textures/Boat.png")
    EndSub
      
    Sub Update 'This is the meat of the game
      PlayerMove() 'Check player input, changes variables playerX and playerY
      Sprites.Move(player, playerX, playerY) 'Actually moves the player sprite to the given position
      For i = 1 To 100 'just some random positions around the center of the window (x=0 y=0)
        x = Math.GetRandomNumber(600) - 300
        y = Math.GetRandomNumber(600) - 300
        Sprites.Move(others[i], x, y)
      EndFor
    EndSub
      
    Sub Stop
      TextWindow.WriteLine("Thanks for, uhh, playing I guess. Bye!")
    EndSub

    Sub PlayerMove
      'IsKeyDown returns a value indicating that the requested key is or isn't down
      If Input.IsKeyDown("W") Then
        playerY = playerY + speed
      EndIf
      If Input.IsKeyDown("S") Then
        playerY = playerY - speed
      EndIf
      If Input.IsKeyDown("A") Then
        playerX = playerX - speed
      EndIf
      If Input.IsKeyDown("D") Then
        playerX = playerX + speed
      EndIf
    EndSub

    others = "" 'array
    player = 0
    playerX = 0
    playerY = 0
    speed = 5
    GameEngine.Start(30) 'Start the game, running at a 30fps cap

*Of course the engine supports such games, but a on demand "Render" might make more sense
[1]: https://github.com/JcBernack/ObjectTK
