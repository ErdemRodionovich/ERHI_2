extends Node3D

var circle_length: int = 20
var count_of_circles: int = 3
var sound_on_click: bool = true
var sound_on_circle: bool = true
var current_step: int = 0
var current_circle: int = 0
var save_history: bool = false
var history: Array[Dictionary]

func _ready() -> void:
	load_game()
	$BeadsController.construct(circle_length)

func _input(event: InputEvent) -> void:
	if event.is_action_released("mouse_left") and not $HUD.is_menu_open() and event.position.y > 172.0:
		$BeadsController.step()
		current_step += 1
		var circle_finished: bool = current_step >= circle_length
		if circle_finished:
			current_circle += 1
			current_step = current_step % circle_length
			if current_circle >= count_of_circles:
				current_step = 0
				current_circle = 0
			$HUD.set_circle(current_circle)
			if sound_on_circle:
				$SoundOnCircle.play()
		_save_step_in_history(circle_finished)
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
	save_history = data.get("save_history", save_history)
	history.clear()
	if data.has("history"):
		for row in data["history"]:
			history.append(row)
	
func _update_HUD() -> void:
	$HUD.set_circle_length(circle_length)
	$HUD.set_count_of_circles(count_of_circles)
	$HUD.set_sound_on_click(sound_on_click)
	$HUD.set_sound_on_circle(sound_on_circle)
	$HUD.set_step(current_step)
	$HUD.set_circle(current_circle)
	$HUD.set_save_history(save_history)
	
func _save():
	return {
		"circle_length": circle_length,
		"count_of_circles": count_of_circles,
		"sound_on_click": sound_on_click,
		"sound_on_circle": sound_on_circle,
		"current_step": current_step,
		"current_circle": current_circle,
		"save_history": save_history,
		"history": history
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
	if what == NOTIFICATION_WM_CLOSE_REQUEST or what == NOTIFICATION_WM_GO_BACK_REQUEST:
		save_game()
		get_tree().quit()
	elif what == NOTIFICATION_APPLICATION_PAUSED:
		save_game()


func _on_hud_save_history_toggled(enabled: bool) -> void:
	save_history = enabled

func _save_step_in_history(with_circle: bool) -> void:
	if not save_history:
		return
	if history.is_empty():
		_add_new_entry_of_history(with_circle)
	else:
		_find_and_add_step_to_last_entry(with_circle)
		
func _add_new_entry_of_history(with_circle: bool) -> void:
	var now = Time.get_datetime_dict_from_system(true)
	var year: int = now["year"]
	var month: int = now["month"]
	var day: int = now["day"]
	var hour: int = now["hour"]
	var step: int = 1
	var circle: int = 1 if with_circle else 0
	history.append({
		"y": year,
		"m": month,
		"d": day,
		"h": hour,
		"st": step,
		"cr": circle
	})

func _find_and_add_step_to_last_entry(with_circle: bool) -> void:
	for i in range(history.size() - 1, -1, -1):
		if _is_in_the_same_last_hour(history[i]):
			history[i]["st"] += 1
			if with_circle:
				history[i]["cr"] += 1
			return
	_add_new_entry_of_history(with_circle)

func _is_in_the_same_last_hour(entry: Dictionary) -> bool:
	var now = Time.get_datetime_dict_from_system(true)
	return (entry["y"] == now["year"] and entry["m"] == now["month"]
		and entry["d"] == now["day"] and entry["h"] == now["hour"])


func _on_hud_request_to_update_history() -> void:
	var overall: int = 0
	var year: int = 0
	var month: int = 0
	var week: int = 0
	var today: int = 0
	var now = Time.get_datetime_dict_from_system(true)
	for entry in history:
		overall += entry.get("st", 0)
		if _is_in_the_same_day(entry, now):
			today += entry.get("st", 0)
		if _is_in_the_same_week(entry, now):
			week += entry.get("st", 0)
		if _is_in_the_same_month(entry, now):
			month += entry.get("st", 0)
		if _in_in_the_same_year(entry, now):
			year += entry.get("st", 0)
			
	$HUD.set_history({
		"today": today,
		"week": week,
		"month": month,
		"year": year,
		"overall": overall
	})
		

func _is_in_the_same_day(entry, now) -> bool:
	return (entry["y"] == now["year"] and entry["m"] == now["month"]
		and entry["d"] == now["day"])

func _is_in_the_same_week(entry, now) -> bool:
	if entry["y"] != now["year"]:
		return false
	return _week_of_year(entry) == _week_of_year(now)
	
func _week_of_year(date: Dictionary) -> int:
	var year: int = date.get("y", date.get("year", 0))
	var month: int = date.get("m", date.get("month", 0))
	var day: int = date.get("d", date.get("day", 0))
	var day_of_week: int = _day_of_week(year, month, day)
	return floori((10.0+_day_of_year(year, month, day)-day_of_week)/7.0)
	
func _day_of_week(year: int, month: int, day: int) -> int:
	var week_day = Time.get_datetime_dict_from_datetime_string("{}-{}-{}T00:00:01".format([year, month, day], "{}"), true)["weekday"]
	if week_day == Time.WEEKDAY_SUNDAY:
		return 7
	return week_day
	
func _day_of_year(year:int, month:int, day:int) -> int:
	var n1: int = floori(275.0 * month / 9.0)
	var n2: int = floori((month+9.0)/12.0)
	var n3: int = (1+floori((year-4*floori(1.0*year/4.0)+2.0)/3.0))
	return n1 - (n2*n3) + day - 30

func _is_in_the_same_month(entry, now) -> bool:
	return entry["y"] == now["year"] and entry["m"] == now["month"]

func _in_in_the_same_year(entry, now) -> bool:
	return entry["y"] == now["year"]
