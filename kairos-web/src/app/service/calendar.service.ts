import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CalendarService {

  private nextEventId: number = 128;
  private events: EventModel[] = [
    { id: 123, title: 'Se présenter aux consultations', description: 'Rattraper les 8h de consultation de la semaine dernière.', assignedTo: 'Gregory House' },
    { id: 124, title: 'Fouiller appartement de Mme Thomson', description: 'Rechercher des traces de moisissures.', assignedTo: 'Eric Foreman' },
    { id: 125, title: 'Remplacer la vicodin par des placebo ', description: 'Voir avec le pharmacien comment procéder exactement.', assignedTo: 'Lisa Cuddy' },
    { id: 126, title: 'Faire la prise de sang à Mme Thomson au labo', description: 'Chambre 706, puis l\'envoyer au labo', assignedTo: 'Eric Foreman' },
    { id: 127, title: 'Faire le test pour la chorée de huntington', description: 'Qu\'on soit fixer une fois pour toute !', assignedTo: 'Numéro 13' }
  ];

  constructor() { }

  getEvents(): EventModel[] {
    return this.events;
  }

  getEvent(eventId: number): EventModel|undefined {
    return this.events.find((event: EventModel) => event.id === eventId);
  }

  createEvent(): EventModel {
    const id: number = this.getNextEventId();
    const event: EventModel = {
      id,
      title: 'Event #' + id
    };
    this.events.push(event);
    
    return event;
  }

  deleteEvent(event: EventModel): void {
    this.events = this.events.filter((item: EventModel) => item.id !== event.id);
  }

  saveEvent(event: EventModel): void {
    // si la event existe dans le tableau
    const eventIndex: number = this.events.indexOf(event);
    eventIndex >= 0 ?
      this.events[eventIndex] = event :  // on la met à jour
      this.events.push(event);          // sinon on l'ajoute
  }

  private getNextEventId(): number {
    const id = this.nextEventId;
    this.nextEventId++;
    return id;
  }
}

export interface EventModel {
  id: number;
  title: string;
  description?: string;
  assignedTo?: string;
  dueDate?: string;
}
