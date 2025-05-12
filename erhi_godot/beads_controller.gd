extends Node3D

@export var Bead_scene: PackedScene

var bead: Node3D
var current_step: int = 0
var overall_steps: int = 50

func _ready() -> void:
	construct(20)

func construct(count: int) -> void:
	# construct Path3D
	var curve = Curve3D.new()
	#curve.add_point(Vector3(0.0, 0.0, 0.0))
	#curve.add_point(Vector3(10.0, 0.0, 0.0))
	#curve.add_point(Vector3(10.0, -20.0, 0.0))
	#curve.add_point(Vector3(-10.0, -20.0, 0.0))
	#curve.add_point(Vector3(-10.0, 0.0, 0.0))
	#curve.add_point(Vector3(0.0, 0.0, 0.0))
	curve.add_point(Vector3(0.0, 0.0, 0.0), Vector3(-10.0, -10.0, 0.0), Vector3(10.0, -10.0, 0.0))
	curve.add_point(Vector3(0.0, -20.0, 0.0), Vector3(10.0, -10.0, 0.0), Vector3(-10.0, -10.0, 0.0))
	
	$Path3D.set_curve(curve)
	
	bead = Bead_scene.instantiate()
	$Path3D.add_child(bead)


func _on_timer_timeout() -> void:
	if not bead:
		return
	current_step = (current_step + 1) % overall_steps
	bead.set_progress_ratio(1.0 * current_step / overall_steps)
	print_debug("bead position is:", bead.position)
