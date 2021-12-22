using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace ET.Module
{
    public class PlayableAnimationComponentAwakeSystem: AwakeSystem<PlayableAnimationComponent>
    {
        public override void Awake(PlayableAnimationComponent self)
        {
            var gameObjectComponent = self.Parent.GetComponent<GameObjectComponent>();
            if (gameObjectComponent==null)
            {
                Log.Error("无法找到GameObjectComponent组件");
                self.Dispose();
                return;
            }

            var animator = gameObjectComponent.GameObject.GetComponent<Animator>();
            if (animator==null)
            {
                Log.Error("无法找到Animator组件");
                self.Dispose();
                return;
            }

            self.Animator = animator;
            var clip = GlobalComponent.Instance.Global.GetComponent<ReferenceCollector>().Get<AnimationClip>("Run");
            self.Play(clip);
        }
    }
    
    public class PlayableAnimationComponentDestorySystem: DestroySystem<PlayableAnimationComponent>
    {
        public override void Destroy(PlayableAnimationComponent self)
        {
            self.Graph.Destroy();
            self.Animator = null;
        }
    }

    public static class PlayableAnimationComponentSystem
    {
        public static void Play(this PlayableAnimationComponent self, AnimationClip clip)
        {
            self.Graph = PlayableGraph.Create();
            self.Graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
            var Output = AnimationPlayableOutput.Create(self.Graph, "Animation", self.Animator);
            AnimationClipPlayable clipPlayable = AnimationClipPlayable.Create(self.Graph, clip);
            Output.SetSourcePlayable(clipPlayable);
            self.Graph.Play();
        }

        public static void PlayBlendTree(this PlayableAnimationComponent self, AnimationClip clip0,AnimationClip clip1)
        {
            // Creates the graph, the mixer and binds them to the Animator.

            self.Graph = PlayableGraph.Create();

            var playableOutput = AnimationPlayableOutput.Create(self.Graph, "Animation", self.Animator);

            var mixerPlayable = AnimationMixerPlayable.Create(self.Graph, 2);
            
            playableOutput.SetSourcePlayable(mixerPlayable);

            // Creates AnimationClipPlayable and connects them to the mixer.

            var clipPlayable0 = AnimationClipPlayable.Create(self.Graph, clip0);

            var clipPlayable1 = AnimationClipPlayable.Create(self.Graph, clip1);

            self.Graph.Connect(clipPlayable0, 0, mixerPlayable, 0);

            self.Graph.Connect(clipPlayable1, 0, mixerPlayable, 1);
            
            // Plays the Graph.

            self.Graph.Play();
        }
        
    }
}