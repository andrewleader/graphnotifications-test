package com.microsoft.aleader.graphnotificationstestandroid

import android.support.v7.app.AppCompatActivity
import android.os.Bundle
import android.view.View
import android.widget.Button
import android.widget.Toast

class MainActivity : AppCompatActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        findViewById<Button>(R.id.ButtonRefresh).setOnClickListener {
            Toast.makeText(this@MainActivity, "Refreshing", Toast.LENGTH_SHORT).show()
        }
    }
}
