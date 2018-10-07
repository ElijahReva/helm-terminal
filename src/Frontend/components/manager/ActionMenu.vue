<template>
    <div>
        <div class="ui secondary  menu">
            <div class="item ">
                <i class="check green icon"></i>    
            </div>            
            <div class="item">
                <div class="ui header  content">
                    {{ currentChart }}
                    <div class="sub header">Last update date</div>
                </div>
            </div>
            <div class="right menu">
                <div class="item">
                    <button class="ui labeled icon button" :class="yamlButtonColor" v-on:click="showYaml">
                        <i class="file outline icon"></i>
                        {{ yamlButtonText }}
                    </button>
                </div>
            </div>
        </div>        
        <form class="ui form error">
            <div class="field yamlForm">
                <div class="field">
                    <label>Values</label>
                    <codemirror
                        name="code"
                        rows="2"
                        v-model="yaml"
                        v-on:update="onYamlChange()"
                        :options="cmOptions" />
                </div>
            </div>
            <div class="ui error message" v-if="hasErrors">
                <div class="header">Invalid yaml!</div>
                <ul class="list">
                    <li>{{ whyYouCant }}</li>
                </ul>
            </div>
            <div class="ui horizontal list">
                <template v-for="action in currentActions">
                    <button 
                        class="ui submit button" 
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
import {CHANGE_YAML_VISIBILITY, VALIDATE_YAML} from "../../store/manager/actions";

import _ from 'lodash'

import { createNamespacedHelpers } from 'vuex'

const { mapState, mapGetters, mapActions, mapMutations} = createNamespacedHelpers('manager');

export default {
    name: "ActionMenu",
    data() {
        return {
            yaml: "",
            cmOptions: {
                tabSize: 2,
                mode: 'text/yaml',
                theme: 'idea',
                lineNumbers: true,
                line: true,
            },            
        }
    },
    computed: {
        ...mapState([
            'currentChart',
            'currentActions',
            'whyYouCant'
        ]),
        ...mapGetters([
            'yamlButtonColor',
            'yamlButtonText',
            'hasErrors'
        ])
    },
    mounted() {
        if(!this.$store.state.manager.isYamlVisible) $('.yamlForm').transition('slide down');
        this.yaml = this.$store.state.manager.currentYaml;        
    },
    methods: {
        ...mapActions([
            VALIDATE_YAML,            
        ]),
        ...mapMutations([
            CHANGE_YAML_VISIBILITY,
        ]),
        showYaml() {
          $('.yamlForm').transition('slide down');
          this.CHANGE_YAML_VISIBILITY();
        },

        onYamlChange: _.throttle(function (e) {
            if(this.yaml !== '' || isNaN(this.yaml)) this.VALIDATE_YAML(this.yaml).catch(console.log);
        }, 3000)  
    },
}
</script>

<style scoped>

</style>