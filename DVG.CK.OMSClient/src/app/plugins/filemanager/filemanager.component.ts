import { Component, OnInit, ViewChild, ElementRef, Output, EventEmitter, AfterViewInit } from '@angular/core';
import { Const } from '../../utils/consts/const';
import { HttpService } from '../../services/http.service';

@Component({
  selector: 'app-filemanager',
  templateUrl: './filemanager.component.html',
  styleUrls: ['./filemanager.component.scss']
})
export class FilemanagerComponent implements OnInit, AfterViewInit {
  constructor(private httpService: HttpService) {
    for (let i = 0; i < 7; i++) {
      const homnay = new Date();
      const ngay = new Date(homnay.setDate(homnay.getDate() - i));
      ngay.setHours(ngay.getHours() + 7);
      const ngaystring = ngay
        .toISOString()
        .substring(0, 10)
        .replace(/-/g, '/');
      const treenode = { id: ngay.toISOString(), name: ngaystring };
      this.treeNodes.push(treenode);
    }
    this.listImages = [];
  }
  public treeNodes = [];
  public listImages: FileInfo[];
  public isBusy = false;
  public fileToUpload: FileList = null;
  @ViewChild('fileInput') fileInput: ElementRef;

  ngOnInit() {
  }

  ngAfterViewInit() {
    this.firstNodeClick();
  }

  public firstNodeClick() {
    document.getElementsByName('spanTreeNode')[0].click();
  }

  public treeNodeClick(node) {
    this.isBusy = true;
    const commonModel = { DayGet: node.data.name };
    this.httpService.DoPost(Const.LoadFileAPI, commonModel).subscribe(msg => {
      if (msg != null && msg.Error === false) {
        this.listImages = <FileInfo[]>msg.Obj;
      } else {
        alert(msg.Title);
      }
      this.isBusy = false;
    });
  }

  public selectImage(image) {
    const isOnIOS = navigator.userAgent.match(/iPad/i) || navigator.userAgent.match(/iPhone/i) || navigator.userAgent.match(/iPod/i);
    if (isOnIOS) {
      window.parent.postMessage(image, '*');
    } else {
      window.opener.postMessage(image);
    }
    window.close();
  }

  public handleFileInput(files: FileList, evt: any) {
    let validImage = true;
    for (var i = 0; i < files.length; i++) {
      if (files[i].size / 1048576 > 4) {
        validImage = false;
        $("#file").val('');
        alert("The image size maximum is 4MB. Please recheck your image size!")
        this.fileToUpload = null;
        return false;
      }
    }
    if (validImage)
      this.fileToUpload = files;
  }

  public uploadFile() {
    this.isBusy = true;
    if (this.fileToUpload === null || this.fileToUpload.length === 0) {
      alert('Please select file!');
      this.isBusy = false;
    } else {
      this.httpService
        .DoPostFileMulti(Const.UpFileAPI, this.fileToUpload)
        .subscribe(msg => {
          if (msg != null && msg.Error === false) {
            this.fileInput.nativeElement.value = null;
            this.firstNodeClick();
          } else {
            alert(msg.Title);
          }
          this.isBusy = false;
        });
    }
  }
}
export class FileInfo {
  public Result: boolean;
  public Name: string;
  public Path: string;
  public FullPath: string;
  public FullOriginalPath: string;
  public Size: number;
  public Extension: string;
  public Message: string;
}
