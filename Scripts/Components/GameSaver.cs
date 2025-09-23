using Godot;

using Ionfall.Scripts.Resources;

namespace Ionfall.Scripts.Components {
	public partial class GameSaver : Node {
		private GameData _dummyGameData;
	
		private const string SaveDir  = "user://Saves";
		private const string SavePath = SaveDir + "/game_save.tres";
		private const string GdSaveScriptPath = "res://Scripts/Resources/GameSave.gd";

		public override void _Ready() {
			_dummyGameData = new GameData { HighScore = 0, Health = 0 };
		}

		public void Save(GameData data) {
			using var dir = DirAccess.Open("user://");
			if (dir == null) { GD.PrintErr("Could not open user://"); return; }
			var mk = dir.MakeDirRecursive("Saves");
			if (mk != Error.Ok && mk != Error.AlreadyExists) GD.PrintErr($"Save dir error: {mk}");

			// Instantiate the GDScript Resource class
			var script = GD.Load<Script>(GdSaveScriptPath);
			if (script == null) { GD.PrintErr("Missing GameSave.gd"); return; }

			// Use Call to invoke "new" on the script
			var res = (Resource)script.Call("new");
			
			// Set exported props
			res.Set("high_score", data.HighScore);
			res.Set("health", data.Health);

			var err = ResourceSaver.Save(res, SavePath);
			if (err != Error.Ok) GD.PrintErr($"Save failed: {err}");
		}

		public GameData Load() {
			if (!ResourceLoader.Exists(SavePath)) return _dummyGameData;

			var res = ResourceLoader.Load<Resource>(
				SavePath, 
				typeHint: null, 
				cacheMode: ResourceLoader.CacheMode.Ignore
			);
		
			if (res == null) { GD.PrintErr("Load failed"); return _dummyGameData; }

			// get data from loaded resource
			var hs = (int)res.Get("high_score");
			var hp = (int)res.Get("health");
			return new GameData { HighScore = hs, Health = hp };
		}

		public void Clear() {
			DirAccess.Open("user://").Remove(SavePath);
		}
	}
}
