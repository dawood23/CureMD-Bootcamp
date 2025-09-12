import { Component, inject, OnInit } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { VisitState } from '../../store/visitNotes/visitNotes.state';
import { visitNotes, CreateVisitRequeset, UpdateVisitRequest } from '../../models/visitNotes';
import { Observable } from 'rxjs';
import { ActivatedRoute, RouterLink } from "@angular/router";
import { GetVisitById, AddVisit, UpdateVisit } from '../../store/visitNotes/visitNotes.actions';
import { AppointmentState } from '../../store/appointments/appointment.state';
import { Appointment } from '../../models/appointment';
import { getAppointmentByID } from '../../store/appointments/appointment.actions';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-visit-note',
  standalone: true,
  imports: [RouterLink, CommonModule, FormsModule],
  templateUrl: './visit-note.component.html',
  styleUrl: './visit-note.component.scss'
})
export class VisitNoteComponent implements OnInit {
  id: any;
  store = inject(Store);
  visit: visitNotes | null = null;
  route = inject(ActivatedRoute);
  currAppointment: Appointment | null = null;

  showPopup = false;
  noteContent = '';

  @Select(VisitState.selectedVisit) selectedVisit$!: Observable<visitNotes>;
  @Select(AppointmentState.currentAppointment) currentAppointment$!: Observable<Appointment>;

  ngOnInit(): void {
    this.route.queryParamMap.subscribe(params => {
      this.id = params.get("appID");
    });

    this.store.dispatch(new getAppointmentByID(this.id));
    this.store.dispatch(new GetVisitById(this.id));

    this.currentAppointment$.subscribe(app => {
      this.currAppointment = app;
    });

    this.selectedVisit$.subscribe(visit => {
      this.visit = visit;
    });
  }

  openPopup() {
    if (this.currAppointment?.status !== "Completed") {
      alert("Appointment must be completed to add or edit a visit note.");
      return;
    }
    this.noteContent = this.visit?.content || ''; 
    this.showPopup = true;
  }

  closePopup() {
    this.showPopup = false;
    this.noteContent = '';
  }

  saveNote() {
    if (!this.noteContent.trim()) {
      alert("Content cannot be empty.");
      return;
    }

    if (this.visit) {
     
      const payload: UpdateVisitRequest = { content: this.noteContent };
      this.store.dispatch(new UpdateVisit(this.visit.visitNoteID, payload)).subscribe(() => {
        this.closePopup();
        this.store.dispatch(new GetVisitById(this.id)); 
      });
    } else {
  
      const payload: CreateVisitRequeset = {
        appointmentID: this.currAppointment?.appointmentID!,
        content: this.noteContent
      };
      this.store.dispatch(new AddVisit(payload)).subscribe(() => {
        this.closePopup();
        this.store.dispatch(new GetVisitById(this.id)); 
      });
    }
  }
}
