extends Control

signal circle_length_updated(length: int)
signal count_of_circles_updated(count: int)
signal sound_on_click_toggled(enabled: bool)
signal sound_on_circle_toggled(enabled: bool)
signal reset
signal save_history_toggled(enabled: bool)
signal request_to_update_history
signal language_selected(language: String)

func _ready() -> void:
	$TabContainer/AboutScreen/VersionLabel.text = "v. " + ProjectSettings.get_setting("application/config/version")
	$TabContainer.hide()
	$TabContainer/MenuScreen/Menu/MenuButton.get_popup().connect("id_pressed", _on_language_selected)

func _check_and_update_circle_length(new_text: String) -> void:
	var circle_length: int = new_text.to_int()
	circle_length = min(5000, circle_length)
	circle_length = max(1, circle_length)
	set_circle_length(circle_length)
	circle_length_updated.emit(circle_length)

func set_circle_length(circle_length: int) -> void:
	$TabContainer/MenuScreen/Menu/CircleLengthEdit.text = str(circle_length)

func _on_circle_length_edit_text_submitted(new_text: String) -> void:
	_check_and_update_circle_length(new_text)


func _on_circle_length_edit_editing_toggled(toggled_on: bool) -> void:
	_check_and_update_circle_length($TabContainer/MenuScreen/Menu/CircleLengthEdit.text)


func _check_and_update_count_of_circles(new_text: String) -> void:
	var count_of_circles: int = new_text.to_int()
	count_of_circles = max(1, count_of_circles)
	set_count_of_circles(count_of_circles)
	count_of_circles_updated.emit(count_of_circles)

func set_count_of_circles(count_of_circles: int) -> void:
	$TabContainer/MenuScreen/Menu/CountOfCirclesCountEdit.text = str(count_of_circles)


func _on_count_of_circles_count_edit_text_submitted(new_text: String) -> void:
	_check_and_update_count_of_circles(new_text)


func _on_count_of_circles_count_edit_editing_toggled(toggled_on: bool) -> void:
	_check_and_update_count_of_circles($TabContainer/MenuScreen/Menu/CountOfCirclesCountEdit.text)


func _on_sound_on_click_button_toggled(toggled_on: bool) -> void:
	sound_on_click_toggled.emit(toggled_on)


func set_sound_on_click(enabled: bool) -> void:
	$TabContainer/MenuScreen/Menu/SoundOnClickButton.set_pressed_no_signal(enabled)


func _on_sound_on_circle_button_toggled(toggled_on: bool) -> void:
	sound_on_circle_toggled.emit(toggled_on)


func set_sound_on_circle(enabled: bool) -> void:
	$TabContainer/MenuScreen/Menu/SoundOnCircleButton.set_pressed_no_signal(enabled)



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
	$TabContainer/MenuScreen/Menu/SaveHistoryButton.set_pressed_no_signal(enabled)


func _on_history_button_pressed() -> void:
	request_to_update_history.emit()
	$TabContainer.current_tab = 3
	
func set_history(history) -> void:
	$TabContainer/HistoryScreen/Today/TodayHistoryLabel.text = str(history.get("today",0))
	$TabContainer/HistoryScreen/Week/ThisWeekHistoryLabel.text = str(history.get("week", 0))
	$TabContainer/HistoryScreen/Month/ThisMonthHistoryLabel.text = str(history.get("month", 0))
	$TabContainer/HistoryScreen/Year/ThisYearHistoryLabel.text = str(history.get("year", 0))
	$TabContainer/HistoryScreen/Overall/OverallHistoryLabel.text = str(history.get("overall", 0))
	
	
func _on_language_selected(index: int) -> void:
	if index < 0:
		return
	$TabContainer/MenuScreen/Menu/MenuButton.text = $TabContainer/MenuScreen/Menu/MenuButton.get_popup().get_item_text(index)
	var language: String = "en"
	if index == 1:
		language = "ru"
	elif index == 2:
		language = "bua"
	elif index == 3:
		language = "bg"
	elif index == 4:
		language = "mn"
	elif index == 5:
		language = "bo"
	elif index == 6:
		language = "cmn"
	elif index == 7:
		language = "es"
	elif index == 8:
		language = "ar"
	elif index == 9:
		language = "fr"
	elif index == 10:
		language = "pt"
	elif index == 11:
		language = "de"
	elif index == 12:
		language = "ko"
	elif index == 13:
		language = "ja"
	
	
	language_selected.emit(language)

func set_language(language: String) -> void:
	var index: int = 0
	if language == "ru":
		index = 1
	elif language == "bua":
		index = 2
	elif language == "bg":
		index = 3
	elif language == "mn":
		index = 4
	elif language == "bo":
		index = 5
	elif language == "cmn":
		index = 6
	elif language == "es":
		index = 7
	elif language == "ar":
		index = 8
	elif language == "fr":
		index = 9
	elif language == "pt":
		index = 10
	elif language == "de":
		index = 11
	elif language == "ko":
		index = 12
	elif language == "ja":
		index = 13
		
	$TabContainer/MenuScreen/Menu/MenuButton.text = $TabContainer/MenuScreen/Menu/MenuButton.get_popup().get_item_text(index)
