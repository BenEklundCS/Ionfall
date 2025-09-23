@tool
extends EditorScript

const SOURCE_ROOT := "res://Assets/Audio/Music/"
const OUTPUT_RANDOMIZER := "res://Assets/Audio/Music/MainRandomizer.tres"
const FILENAME_FILTER := "Main"
const STREAM_WEIGHT := 1.0

func _run() -> void:
	var matching_paths := _collect_matching_streams(SOURCE_ROOT)
	
	if matching_paths.is_empty():
		push_warning("No matching audio files found under %s" % SOURCE_ROOT)
		return

	var randomizer := AudioStreamRandomizer.new()
	var stream_index := 0
	
	for audio_path in matching_paths:
		var stream := ResourceLoader.load(audio_path)
		if stream == null or not (stream is AudioStream):
			continue
		randomizer.add_stream(stream_index, stream, STREAM_WEIGHT)
		stream_index += 1

	if stream_index == 0:
		push_warning("All matching files failed to load; nothing to save.")
		return

	var save_err := ResourceSaver.save(randomizer, OUTPUT_RANDOMIZER)
	if save_err != OK:
		push_error("Failed to save AudioStreamRandomizer to %s (error %d)" % [OUTPUT_RANDOMIZER, save_err])
		return

	print("Saved AudioStreamRandomizer to %s with %d tracks." % [OUTPUT_RANDOMIZER, stream_index])

func _collect_matching_streams(root_path: String) -> Array[String]:
	var results: Array[String] = []
	_scan_directory(root_path, results)
	results.sort()
	return results

func _scan_directory(path: String, accumulator: Array[String]) -> void:
	var dir := DirAccess.open(path)
	if dir == null:
		return

	dir.list_dir_begin()
	while true:
		var name := dir.get_next()
		if name == "":
			break
		if name == "." or name == "..":
			continue

		var full_path := path.path_join(name)
		if dir.current_is_dir():
			_scan_directory(full_path, accumulator)
			continue

		var lower_name := name.to_lower()
		var is_audio_file := lower_name.ends_with(".mp3") or lower_name.ends_with(".ogg") or lower_name.ends_with(".wav")
		var has_filter := lower_name.find(FILENAME_FILTER.to_lower()) != -1
		
		if is_audio_file and has_filter:
			accumulator.append(full_path)
             
	dir.list_dir_end()
