extends Control

signal circle_length_updated(length: int)
signal count_of_circles_updated(count: int)
signal sound_on_click_toggled(enabled: bool)
signal sound_on_circle_toggled(enabled: bool)
signal reset
signal save_history_toggled(enabled: bool)
signal request_to_update_history

func _ready() -> void:
	$TabContainer/AboutScreen/VersionLabel.text = "v. " + ProjectSettings.get_setting("application/config/version")
	$TabContainer.hide()

func _check_and_update_circle_length(new_text: String) -> void:
	var circle_length: int = new_text.to_int()
	circle_length = min(5000, circle_length)
	circle_length = max(1, circle_length)
	set_circle_length(circle_length)
	circle_length_updated.emit(circle_length)

func set_circle_length(circle_length: int) -> void:
	$TabContainer/MenuScreen/VBoxContainer/Menu/CircleLengthEdit.text = str(circle_length)

func _on_circle_length_edit_text_submitted(new_text: String) -> void:
	_check_and_update_circle_length(new_text)


func _on_circle_length_edit_editing_toggled(toggled_on: bool) -> void:
	_check_and_update_circle_length($TabContainer/MenuScreen/VBoxContainer/Menu/CircleLengthEdit.text)


func _check_and_update_count_of_circles(new_text: String) -> void:
	var count_of_circles: int = new_text.to_int()
	count_of_circles = max(1, count_of_circles)
	set_count_of_circles(count_of_circles)
	count_of_circles_updated.emit(count_of_circles)

func set_count_of_circles(count_of_circles: int) -> void:
	$TabContainer/MenuScreen/VBoxContainer/Menu/CountOfCirclesCountEdit.text = str(count_of_circles)


func _on_count_of_circles_count_edit_text_submitted(new_text: String) -> void:
	_check_and_update_count_of_circles(new_text)


func _on_count_of_circles_count_edit_editing_toggled(toggled_on: bool) -> void:
	_check_and_update_count_of_circles($TabContainer/MenuScreen/VBoxContainer/Menu/CountOfCirclesCountEdit.text)


func _on_sound_on_click_button_toggled(toggled_on: bool) -> void:
	sound_on_click_toggled.emit(toggled_on)


func set_sound_on_click(enabled: bool) -> void:
	$TabContainer/MenuScreen/VBoxContainer/Menu/SoundOnClickButton.set_pressed_no_signal(enabled)


func _on_sound_on_circle_button_toggled(toggled_on: bool) -> void:
	sound_on_circle_toggled.emit(toggled_on)


func set_sound_on_circle(enabled: bool) -> void:
	$TabContainer/MenuScreen/VBoxContainer/Menu/SoundOnCircleButton.set_pressed_no_signal(enabled)



func _on_about_button_pressed() -> void:
	$TabContainer.current_tab = 2


func _on_back_button_pressed() -> void:
	$TabContainer.current_tab = 1


func _on_menu_button_pressed() -> void:
	$TabContainer.current_tab = 1
	if $TabContainer.is_visible_in_tree():
		$TabContainer.hide()
	else:
		$TabContainer.show()


func _on_author_label_meta_clicked(meta: Variant) -> void:
	OS.shell_open(str(meta))


func _on_about_rich_text_label_meta_clicked(meta: Variant) -> void:
	OS.shell_open(str(meta))

func is_menu_open() -> bool:
	return $TabContainer.is_visible_in_tree()
	
func set_step(step: int) -> void:
	$StepLabel.text = str(step)
	
func set_circle(circle: int) -> void:
	$StepLabel/CircleLabel.text = str(circle)


func _on_reset_button_pressed() -> void:
	reset.emit()


func _on_save_history_button_toggled(toggled_on: bool) -> void:
	save_history_toggled.emit(toggled_on)

func set_save_history(enabled: bool) -> void:
	$TabContainer/MenuScreen/VBoxContainer/Menu/SaveHistoryButton.set_pressed_no_signal(enabled)


func _on_history_button_pressed() -> void:
	request_to_update_history.emit()
	$TabContainer.current_tab = 3
	
func set_history(history) -> void:
	$TabContainer/HistoryScreen/TodayHistoryLabel.text = "Today: " + str(history.get("today",0))
	$TabContainer/HistoryScreen/ThisWeekHistoryLabel.text = "This week: " + str(history.get("week", 0))
	$TabContainer/HistoryScreen/ThisMonthHistoryLabel.text = "This month: " + str(history.get("month", 0))
	$TabContainer/HistoryScreen/ThisYearHistoryLabel.text = "This year: " + str(history.get("year", 0))
	$TabContainer/HistoryScreen/OverallHistoryLabel.text = "Overall: " + str(history.get("overall", 0))
	
	
