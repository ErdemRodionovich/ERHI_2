extends Node3D

@export var Bead_scene: PackedScene

var bead: Node3D
var last_bead: Node3D
var steps_to_forward: int = 0
var vacant_places: Array[float]
var normal_speed: float = 1.0
var speed: float = normal_speed
var high_speed: float = 5.0
var free_places_count: int = 5
var place_of_last_bead: float = 0.0
var normal_wait_time: int = 100

func construct(count: int) -> void:
	get_tree().call_group("beads", "queue_free")
	
	var curve = Curve3D.new()
	var height: float = maxf(0.8 * count, 10.0)
	var half_width: float = maxf(5.0, sqrt(height*4.0))
	curve.add_point(Vector3(0.0, 0.0, 0.0), Vector3(half_width/2.0, 0.0, 0.0), Vector3(-half_width/2.0, 0.0, 0.0))
	curve.add_point(Vector3(0.0, -height, 0.0), Vector3(-half_width, 0.0, 0.0), Vector3(half_width, 0.0, 0.0))
	curve.add_point(Vector3(0.0, 0.0, 0.0), Vector3(half_width/2.0, 0.0, 0.0), Vector3(-half_width/2.0, 0.0, 0.0))
	$Path3D.set_curve(curve)
	
	var bead_set: bool = false
	free_places_count = maxi(5, count/20)
	for i in range(0, count):
		var current = Bead_scene.instantiate()
		$Path3D.add_child(current)
		current.progress_ratio = 1.0 * (count + free_places_count - i) / (count + free_places_count)
		if bead_set:
			last_bead.next_bead = current
		last_bead = current
		if not bead_set:
			bead = current
			bead_set = true
	
	last_bead.next_bead = self
	vacant_places.clear()
	for i in range(0, free_places_count):
		vacant_places.push_back(1.0 * (i + 1)/(count + free_places_count))
	
	place_of_last_bead = last_bead.progress_ratio
	high_speed = curve.get_baked_length()
	

func add_vacant_place(place: float) -> void:
	if (steps_to_forward > 0 or place > place_of_last_bead or is_equal_approx(place, 0.0)) and bead:
		bead.add_vacant_place(place)
		steps_to_forward -= 1
		return
	vacant_places.push_back(place)


func step() -> void:
	if not bead or not last_bead:
		return
	steps_to_forward += 1
	
	if vacant_places.is_empty():
		if speed < high_speed:
			_set_speed(high_speed)
			_set_wait_time(0)
	else:
		if speed > normal_speed:
			_set_speed(normal_speed)
			_set_wait_time(normal_wait_time)
		
	while not vacant_places.is_empty():
		var n: int = 0
		for place in vacant_places:
			bead.add_vacant_place(place)
			n += 1
			
		for i in range(0, n):
			vacant_places.pop_front()
			
	last_bead.next_bead = bead
	last_bead = bead
	bead = bead.next_bead
	last_bead.next_bead = self

func _set_speed(new_speed: float) -> void:
	get_tree().call_group("beads", "update_speed", new_speed)
	speed = new_speed
	
func _set_wait_time(msecs: int) -> void:
	get_tree().call_group("beads", "update_wait_time", msecs)
