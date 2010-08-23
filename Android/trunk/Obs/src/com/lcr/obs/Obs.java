package com.lcr.obs;

import android.app.Activity;

import android.content.Intent;
import android.os.Bundle;
import android.view.MenuItem;
import android.widget.Button;
import android.view.*;
import android.view.View.OnClickListener;

public class Obs extends Activity {
    /** Called when the activity is first created. */
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.main);
        
        Button button = (Button)findViewById(R.id.newObsButton);        
        button.setOnClickListener(newObsListener);    
    }
 
    // Create an anonymous implementation of OnClickListener
    private OnClickListener newObsListener = new OnClickListener() 
    {    
    	public void onClick(View v) 
    	{      
    		
    	}
    };
        
	@Override
	public boolean onMenuItemSelected(int featureId, MenuItem item) {
		// TODO Auto-generated method stub
		return super.onMenuItemSelected(featureId, item);
	}

	@Override
	public boolean onOptionsItemSelected(MenuItem item) {
		// TODO Auto-generated method stub
		return super.onOptionsItemSelected(item);
	}

	@Override
	protected void onStart() {
		// TODO Auto-generated method stub
		super.onStart();
	}

	@Override
	protected void onStop() {
		// TODO Auto-generated method stub
		super.onStop();
	}


	@Override
	public void openContextMenu(View view) {
		// TODO Auto-generated method stub
		super.openContextMenu(view);
	}

	@Override
	public void registerForContextMenu(View view) {
		// TODO Auto-generated method stub
		super.registerForContextMenu(view);
	}

	@Override
	public void startActivityFromChild(Activity child, Intent intent,
			int requestCode) {
		// TODO Auto-generated method stub
		
		super.startActivityFromChild(child, intent, requestCode);
	}

	@Override
	public void onUserInteraction() {
		// TODO Auto-generated method stub
		super.onUserInteraction();
	}
}