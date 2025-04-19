import { Component, inject } from '@angular/core';
import { LikesService } from '../_services/likes.service';
import { Member } from '../_models/member';

@Component({
  selector: 'app-lists',
  standalone: true,
  imports: [],
  templateUrl: './lists.component.html',
  styleUrl: './lists.component.css'
})
export class ListsComponent {
  private likesService = inject(LikesService);
  members: Member[] = [];
  predicate = "liked";

  ngOnInit(): void{
    this.loadLikes();
  }

  
  
  getTitle() {
    switch (this.predicate.toLowerCase()) {
      case "liked": return "Miembros que te gustan";
      case "likedby": return "Miembros a quienes les gustas";
      default: return "Match";
    }
  }

  loadLikes() {
    this.likesService.getLikes(this.predicate).subscribe({
      next: members => this.members = this.members
    })
  }
  
}
