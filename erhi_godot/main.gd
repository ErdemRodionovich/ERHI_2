extends Node3D

func _ready() -> void:
	$BeadsController.construct(20)

func _input(event: InputEvent) -> void:
	if event.is_action_released("mouse_left"):
		$BeadsController.step()
