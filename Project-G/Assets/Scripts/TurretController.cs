﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour {
	private int _environmentLayer = 9;
	private int _playerLayer = 10;
	private Transform _shootPoint;
	[SerializeField] private WeaponBase _weapon = null;
	private GameObject _target;
	private LayerMask _hittableLayersByEnemy;

	// Start is called before the first frame update
	void Start() {
		_shootPoint = _weapon.transform.GetChild(0);
		_hittableLayersByEnemy = (1 << _playerLayer) | (1 << _environmentLayer);

		_target = GameObject.FindWithTag("Player");
	}

	private void FixedUpdate() {
		CheckPlayerInRange();
	}

	private void CheckPlayerInRange() {
		if (_target != null) {
			Vector3 direction = _target.transform.position - transform.position;
			direction.y -= .23f;
			RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, direction, 4f, _hittableLayersByEnemy);
			//Debug.DrawRay(transform.position, direction, Color.blue, .1f);
	
			if (hitInfo.collider != null) {
				if (hitInfo.collider.tag == "Player") {
					Shoot();
				}
			}
		}
	}

	private void Shoot() {
		if (_target != null) {
			// Getting player position
			Vector3 targetPos = new Vector3(_target.transform.position.x, _target.transform.position.y - 0.2f, _target.transform.position.z);
			Vector3 aimDirection = (targetPos - _weapon.transform.position).normalized;
			// Rotating the current weapon
			float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
			_weapon.transform.eulerAngles = new Vector3(0, 0, angle);

			_weapon.WeaponUpdate();
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag("Player")) {
			Shoot();
		}
	}

	private void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.CompareTag("Player")) {
			Shoot();
		}
	}
}
