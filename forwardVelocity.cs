//get forward vector of velocity of object
var localVelocity = transform.InverseTransformDirection(rigidbody.Velocity)
//and then use 
localVelocity.z

//var forwardVelocity = object.forward * Vector3.Dot(object.forward, rigidbody.velocity);