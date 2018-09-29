<template>
    <div class="sixteen wide column">
        <div class="ui secondary  menu">
            <div class="item ">
                <i class="check green icon"></i>    
            </div>            
            <div class="item">
                <div class="ui header  content">
                    Chart Name
                    <div class="sub header">Last update date</div>
                </div>
            </div>
            <div class="right menu">
                <div class="item">
                    <button class="ui labeled icon button" :class="yamlButtonColor" v-on:click="showYaml">
                        <i class="file outline icon"></i>
                        Show yaml
                    </button>
                </div>
            </div>
        </div>        
        <form class="ui form warning">
            <div class="field yamlForm">
                <div class="field">
                    <label>Values</label>
                    <codemirror
                            rows="2"
                            :value="code"
                            :options="cmOptions"
                    />
                </div>
            </div>
            <div class="ui warning message">
                <div class="header">Invalid yaml!</div>
                <ul class="list">
                    <li>That e-mail has been subscribed, but you have not yet clicked the verification link in your e-mail.</li>
                </ul>
            </div>
            <div class="ui horizontal list">
                <template v-for="action in actions">
                    <button 
                        class="ui button" 
                        :class="action.color" 
                        :type="action.type">
                            {{ action.text }}
                    </button>
                </template>
            </div>
        </form>
    </div>
</template>

<script>
import 'codemirror/mode/yaml/yaml'
import 'codemirror/theme/idea.css'

    
export default {
    name: "ActionMenu",
    data() {
        return {
            yamlClicked: false,
            code: 'const:\n  test: 123',
            cmOptions: {
                tabSize: 2,
                mode: 'text/yaml',
                theme: 'idea',
                lineNumbers: true,
                line: true
            },
            actions: [
                { text: 'dd', type: "dd", color: "blue"},
                { text: 'dd2', type: "ddsda", color: "red"},
                { text: 'dd4', type: "ddq", color: "green"},
            ]
        }
    },
    computed: {
        yamlButtonColor() {
            return this.yamlClicked ? 'green' : 'gray';
        }
    },
    methods: {
        showYaml() {
          $('.yamlForm').transition('slide down');
          this.yamlClicked = !(this.yamlClicked);
        }
    }
}
</script>

<style scoped>

</style>