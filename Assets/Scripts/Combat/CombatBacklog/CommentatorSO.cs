using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Vanaring
{

    [Serializable]
    public class Comment
    {
        [SerializeField]
        private string _comment;

        public string GetComment(CombatEntity characterInConversation)
        {
            string characterName = characterInConversation.CharacterSheet.CharacterName;
            return _comment.Replace("[CharacterName]", characterName);
        }
    }
    [CreateAssetMenu(fileName = "CommentatorSO", menuName = "ScriptableObject/CombatBacklog/Commentator")]
    public class CommentatorSO : ScriptableObject
    {
        [SerializeField]
        private List<Comment> _comments_on_characterStun = new List<Comment>();

        [SerializeField]
        private List<Comment> _comments_on_enemyDie = new List<Comment>();

        public List<Comment> GetCommentsOnCharacterStun => _comments_on_characterStun;
        public List<Comment> GetCommentsOnEnemyDie => _comments_on_enemyDie;
        
        public CommentatorRuntime CreateCommentatorRuntime()
        {
            return new CommentatorRuntime(this); 
        }

    }

    public class CommentatorRuntime  {

        [SerializeField]
        private Queue<Comment> _comments_on_characterStun = new Queue<Comment>() ;

        [SerializeField]
        private Queue<Comment> _comments_on_enemyDie    = new Queue<Comment>() ; 

        public CommentatorRuntime(CommentatorSO commentator)
        {
            foreach (var comment in commentator.GetCommentsOnCharacterStun)
            {
                _comments_on_characterStun.Enqueue(comment);
            }
            foreach (var comment in commentator.GetCommentsOnEnemyDie)
            {
                _comments_on_enemyDie.Enqueue(comment); 
            }

            RandomQueueComment(); 
        }

        private void RandomQueueComment()
        {
            int randomQueueTime = UnityEngine.Random.Range(0,_comments_on_enemyDie.Count * 2 ) ; 
            for (int i  = 0; i < randomQueueTime; i++)
            {
                _comments_on_characterStun.Enqueue(_comments_on_characterStun.Dequeue());
                _comments_on_enemyDie.Enqueue(_comments_on_enemyDie.Dequeue());
            }
            

        }

        public Comment GetCharacterStunComment()
        {
            Comment comment = _comments_on_characterStun.Dequeue();
            _comments_on_characterStun.Enqueue(comment);

            return comment;
        }

        public Comment GetOnEnemyDieComment()
        {
            Comment comment = _comments_on_characterStun.Dequeue();
            _comments_on_enemyDie.Enqueue(comment);

            return comment;
        }

    }

}
