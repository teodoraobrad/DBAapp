import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { SharedService } from '../shared.service';

@Component({
  selector: 'app-sqladmin',
  templateUrl: './sqladmin.component.html',
  styleUrls: ['./sqladmin.component.css']
})
export class SqladminComponent implements OnInit {

  constructor(private sharedService: SharedService,
    private router: Router,
    private fb: FormBuilder) { }

  ngOnInit(): void {
  }

}
