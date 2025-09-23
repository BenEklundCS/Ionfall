@tool
extends EditorScript

# --- CONFIG ---
const SHEET_PATH: String = "res://Assets/Sprites/Character/char-sheet-alpha.png"
const ROWS: int = 48
const COLS: int = 8
const IDLE_COL: int = 0
const CROUCH_COL: int = 1
const RUN_START_COL: int = 2   # inclusive
const RUN_FRAMES: int = 4      # cols 2,3,4,5
const SAVE_PATH: String = "res://Assets/Sprites/Generated/char_frames.tres"

# Crops
const CROUCH_CROP_TOP: int = 2
const RUN_CROP_TOP: int = 2
const RUN_CROP_RIGHT: int = 4
# -------------

func _run() -> void:
	var tex: Texture2D = load(SHEET_PATH) as Texture2D
	if tex == null:
		push_error("Couldn't load Texture2D at %s" % SHEET_PATH)
		return

	# Grid cell size (48 x 8)
	var cell_w: int = int(tex.get_width() / float(COLS))
	var cell_h: int = int(tex.get_height() / float(ROWS))
	if cell_w <= 0 or cell_h <= 0:
		push_error("Invalid cell size (%dx%d)." % [cell_w, cell_h])
		return
	if RUN_START_COL < 0 or RUN_START_COL + RUN_FRAMES > COLS:
		push_error("Run window out of bounds.")
		return

	var frames: SpriteFrames = SpriteFrames.new()

	# idle: 48 frames (column 0 across rows) — no crop
	frames.add_animation("idle")
	frames.set_animation_loop("idle", true)
	for r in range(ROWS):
		var x_i: int = IDLE_COL * cell_w
		var y_i: int = r * cell_h
		var w_i: int = cell_w
		var h_i: int = cell_h
		frames.add_frame("idle", _atlas(tex, x_i, y_i, w_i, h_i))

	# crouch: 48 frames (column 1 across rows) — crop 2px from top
	frames.add_animation("crouch")
	frames.set_animation_loop("crouch", true)
	for r in range(ROWS):
		var x_c: int = CROUCH_COL * cell_w
		var y_c: int = r * cell_h + CROUCH_CROP_TOP
		var w_c: int = cell_w
		var h_c: int = cell_h - CROUCH_CROP_TOP
		if h_c < 0:
			h_c = 0
		frames.add_frame("crouch", _atlas(tex, x_c, y_c, w_c, h_c))

	# run_0..run_47: 4 frames each from columns 2..5 — crop 2px top, 4px right
	for r in range(ROWS):
		var anim_name: String = "run_%d" % r
		frames.add_animation(anim_name)
		frames.set_animation_loop(anim_name, true)

		for i in range(RUN_FRAMES):
			var col: int = RUN_START_COL + i
			var x_r: int = col * cell_w
			var y_r: int = r * cell_h + RUN_CROP_TOP
			var w_r: int = cell_w - RUN_CROP_RIGHT
			var h_r: int = cell_h - RUN_CROP_TOP
			if w_r < 0:
				w_r = 0
			if h_r < 0:
				h_r = 0
			frames.add_frame(anim_name, _atlas(tex, x_r, y_r, w_r, h_r))

	# ensure target dir exists
	var target_dir: String = SAVE_PATH.get_base_dir()
	var root: DirAccess = DirAccess.open("res://")
	if root == null:
		push_error("Can't open res://")
		return
	var sub: String = target_dir.replace("res://", "")
	if sub != "":
		var mk: int = root.make_dir_recursive(sub)
		if mk != OK and mk != ERR_ALREADY_EXISTS and mk != ERR_ALREADY_IN_USE:
			push_error("Can't create directory %s (err %d)" % [target_dir, mk])
			return

	# overwrite if present
	if FileAccess.file_exists(SAVE_PATH):
		DirAccess.remove_absolute(ProjectSettings.globalize_path(SAVE_PATH))

	var err: int = ResourceSaver.save(frames, SAVE_PATH)
	if err == OK:
		print("✅ Saved: idle(48), crouch(48, -2 top), run_0..47(4 each, -2 top, -4 right) -> %s" % SAVE_PATH)
	else:
		push_error("❌ Failed to save resource, err=%d" % err)

func _atlas(tex: Texture2D, x: int, y: int, w: int, h: int) -> AtlasTexture:
	var a: AtlasTexture = AtlasTexture.new()
	a.atlas = tex
	a.region = Rect2i(x, y, w, h)
	a.filter_clip = true
	return a
