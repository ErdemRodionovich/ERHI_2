extends PathFollow3D

@export var next_bead: Node3D

var vacant_places: Array[float]
var speed: float = 1.0
var my_place_forwarded: bool = false
var start_progress: float = -1.0
var start_of_movement: int = 0
var wait_duration_msecs: int = 100

func add_vacant_place(place: float) -> void:
	vacant_places.push_back(place)

func _process(delta: float) -> void:
	if vacant_places.is_empty():
		return
		
	var target: float = vacant_places.front()
	if is_equal_approx(target, 0.0):
		target = 1.0
	
	if not my_place_forwarded:
		if start_progress == -1.0:
			start_progress = progress_ratio
			start_of_movement = Time.get_ticks_msec()
		if Time.get_ticks_msec() - start_of_movement >= wait_duration_msecs and next_bead:
			next_bead.add_vacant_place(start_progress)
			my_place_forwarded = true
	
	if is_equal_approx(progress_ratio, 1.0):
		progress_ratio = 0.0
	
	if progress_ratio >= target or is_equal_approx(target, 1.0) and is_equal_approx(progress_ratio, 0.0) or is_equal_approx(progress_ratio, target):
		if next_bead:
			if not my_place_forwarded:
				next_bead.add_vacant_place(start_progress)
				my_place_forwarded = true
		vacant_places.pop_front()
		start_progress = -1.0
		my_place_forwarded = false
	else:
		var new_progress: float = progress_ratio + speed * delta
		if new_progress < target:
			progress_ratio = new_progress
		else:
			progress_ratio = target
	
func update_speed(new_speed: float) -> void:
	speed = new_speed
	
func update_wait_time(msecs: int) -> void:
	wait_duration_msecs = msecs
