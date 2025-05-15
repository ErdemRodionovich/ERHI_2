extends Node3D

var circle_length: int = 20
var count_of_circles: int = 3
var sound_on_click: bool = true
var sound_on_circle: bool = true
var current_step: int = 0
var current_circle: int = 0

func _ready() -> void:
	load_game()
	$BeadsController.construct(circle_length)

func _input(event: InputEvent) -> void:
	if event.is_action_released("mouse_left") and not $HUD.is_menu_open() and event.position.y > 70.0:
		$BeadsController.step()
		current_step += 1
		if current_step >= circle_length:
			current_circle += 1
			current_step = current_step % circle_length
			if current_circle >= count_of_circles:
				current_step = 0
				current_circle = 0
			$HUD.set_circle(current_circle)
			if sound_on_circle:
				$SoundOnCircle.play()
		$HUD.set_step(current_step)
		if sound_on_click:
			$SoundOnClick.play()

func _save_game_filepath() -> String:
	return "user://erhi.save"

func load_game() -> void:
	if not FileAccess.file_exists(_save_game_filepath()):
		_update_HUD()
		return
		
	var save_file = FileAccess.open(_save_game_filepath(), FileAccess.READ)
	var json = JSON.new()
	if not json.parse(save_file.get_line()) == OK:
		return
	_load(json.data)
	_update_HUD()
	
func _load(data: Dictionary) -> void:
	circle_length = data.get("circle_length", circle_length)
	count_of_circles = data.get("count_of_circles", count_of_circles)
	sound_on_click = data.get("sound_on_click", sound_on_click)
	sound_on_circle = data.get("sound_on_circle", sound_on_circle)
	current_step = data.get("current_step", current_step)
	current_circle = data.get("current_circle", current_circle)
	
func _update_HUD() -> void:
	$HUD.set_circle_length(circle_length)
	$HUD.set_count_of_circles(count_of_circles)
	$HUD.set_sound_on_click(sound_on_click)
	$HUD.set_sound_on_circle(sound_on_circle)
	$HUD.set_step(current_step)
	$HUD.set_circle(current_circle)
	
func _save():
	return {
		"circle_length": circle_length,
		"count_of_circles": count_of_circles,
		"sound_on_click": sound_on_click,
		"sound_on_circle": sound_on_circle,
		"current_step": current_step,
		"current_circle": current_circle
	}
	
func save_game() -> void:
	var save_file = FileAccess.open(_save_game_filepath(), FileAccess.WRITE)
	save_file.store_line(JSON.stringify(_save()))


func _on_hud_circle_length_updated(length: int) -> void:
	if circle_length != length:
		circle_length = length
		reset_state()

func reset_state() -> void:
	current_step = 0
	$HUD.set_step(current_step)
	current_circle = 0
	$HUD.set_circle(current_circle)
	$BeadsController.construct(circle_length)


func _on_hud_count_of_circles_updated(count: int) -> void:
	count_of_circles = count
	


func _on_hud_sound_on_circle_toggled(enabled: bool) -> void:
	sound_on_circle = enabled


func _on_hud_sound_on_click_toggled(enabled: bool) -> void:
	sound_on_click = enabled

func _notification(what):
	if what == NOTIFICATION_WM_CLOSE_REQUEST or what == NOTIFICATION_APPLICATION_PAUSED or what == NOTIFICATION_WM_GO_BACK_REQUEST:
		save_game()
		get_tree().quit()
